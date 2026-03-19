namespace EcommerceEnterprise.Domain.Exceptions;

public class InvalidOrderStateException(string message) : DomainException(message) { }