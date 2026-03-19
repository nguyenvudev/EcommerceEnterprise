using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Enums;

namespace EcommerceEnterprise.Domain.Entities.Users;

public class User : AggregateRoot
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public UserRole Role { get; private set; } = UserRole.Customer;
    public bool IsActive { get; private set; } = true;

    protected User() { }

    public static User Create(string email, string passwordHash,
        string fullName, string? phone = null, UserRole role = UserRole.Customer)
        => new()
        {
            Email = email.ToLower(),
            PasswordHash = passwordHash,
            FullName = fullName,
            PhoneNumber = phone,
            Role = role
        };

    public void ChangePassword(string newHash) => PasswordHash = newHash;
    public void Deactivate() => IsActive = false;
}