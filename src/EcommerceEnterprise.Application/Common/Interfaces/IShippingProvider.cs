namespace EcommerceEnterprise.Application.Common.Interfaces;

public interface IShippingProvider
{
    Task<ShippingRateResult> CalculateFeeAsync(
        ShippingRateRequest request,
        CancellationToken ct = default);

    Task<string> CreateShipmentAsync(
        CreateShipmentRequest request,
        CancellationToken ct = default);

    Task<ShipmentTrackingResult> TrackShipmentAsync(
        string trackingCode,
        CancellationToken ct = default);
}

public record ShippingRateRequest(
    string FromProvince,
    string ToProvince,
    string ToDistrict,
    string ToWard,
    decimal Weight);

public record ShippingRateResult(
    decimal Fee,
    int EstimatedDays);

public record CreateShipmentRequest(
    Guid OrderId,
    string RecipientName,
    string RecipientPhone,
    string Address,
    decimal Weight);

public record ShipmentTrackingResult(
    string Status,
    string Description,
    DateTime UpdatedAt);