namespace EcommerceEnterprise.Domain.ValueObjects;

public record Address(
    string FullName,
    string PhoneNumber,
    string Province,
    string District,
    string Ward,
    string StreetAddress);