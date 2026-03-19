namespace EcommerceEnterprise.Domain.Exceptions;

public class InsufficientStockException(string sku, int requested, int available)
    : DomainException($"Không đủ hàng cho SKU '{sku}'. Yêu cầu: {requested}, Còn lại: {available}")
{ }