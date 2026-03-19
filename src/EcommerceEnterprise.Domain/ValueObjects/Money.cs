namespace EcommerceEnterprise.Domain.ValueObjects;

public record Money(decimal Amount, string Currency = "VND")
{
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount, a.Currency);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount, a.Currency);
    public static Money Zero => new(0);
}