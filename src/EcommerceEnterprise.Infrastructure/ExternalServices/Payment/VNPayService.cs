using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EcommerceEnterprise.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EcommerceEnterprise.Infrastructure.ExternalServices.Payment
{
    public class VNPayService : IPaymentGateway
    {
        private readonly string _tmnCode;
        private readonly string _hashSecret;
        private readonly string _baseUrl;
        private readonly string _returnUrl;

        public VNPayService(IConfiguration config)
        {
            _tmnCode = config["VNPay:TmnCode"]!;
            _hashSecret = config["VNPay:HashSecret"]!;
            _baseUrl = config["VNPay:BaseUrl"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            _returnUrl = config["VNPay:ReturnUrl"]!;
        }

        public Task<string> CreatePaymentUrlAsync(Guid orderId, decimal amount, string orderInfo, CancellationToken ct = default)
        {
            var vnpParams = new SortedDictionary<string, string>
            {
                ["vnp_Version"] = "2.1.0",
                ["vnp_Command"] = "pay",
                ["vnp_TmnCode"] = _tmnCode,
                ["vnp_Amount"] = ((long)(amount * 100)).ToString(),
                ["vnp_CurrCode"] = "VND",
                ["vnp_TxnRef"] = orderId.ToString()[..8],
                ["vnp_OrderInfo"] = orderInfo,
                ["vnp_OrderType"] = "billpayment",
                ["vnp_Locale"] = "vn",
                ["vnp_ReturnUrl"] = _returnUrl,
                ["vnp_IpAddr"] = "127.0.0.1",
                ["vnp_CreateDate"] = DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss")
            };

            var query = string.Join("&", vnpParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var signature = HmacSha512(_hashSecret, query);
            var url = $"{_baseUrl}?{query}&vnp_SecureHash={signature}";

            return Task.FromResult(url);
        }

        public Task<PaymentCallbackResult> VerifyCallbackAsync(IDictionary<string, string> parameters)
        {
            // Lấy secure hash
            parameters.TryGetValue("vnp_SecureHash", out var secureHash);
            secureHash ??= "";

            // Lọc bỏ vnp_SecureHash rồi sort
            var filtered = parameters
                .Where(kv => kv.Key.StartsWith("vnp_") && kv.Key != "vnp_SecureHash")
                .OrderBy(kv => kv.Key);

            var query = string.Join("&", filtered.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var expectedHash = HmacSha512(_hashSecret, query);

            // Lấy các giá trị cần kiểm tra
            parameters.TryGetValue("vnp_ResponseCode", out var responseCode);
            parameters.TryGetValue("vnp_TransactionNo", out var txnNo);

            var isSuccess = expectedHash == secureHash && responseCode == "00";

            return Task.FromResult(new PaymentCallbackResult(
                isSuccess,
                txnNo ?? "",
                string.Join("|", parameters.Select(kv => $"{kv.Key}={kv.Value}"))
            ));
        }

        private static string HmacSha512(string key, string data)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}