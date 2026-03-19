using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Exceptions;

namespace EcommerceEnterprise.Domain.Entities.Users;

public class Wallet : BaseEntity
{
    public Guid UserId { get; private set; }
    public decimal Balance { get; private set; }

    protected Wallet() { }

    public static Wallet Create(Guid userId) => new() { UserId = userId, Balance = 0 };

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Số tiền nạp phải lớn hơn 0.");
        Balance += amount;
    }

    public void Debit(decimal amount)
    {
        if (amount > Balance)
            throw new DomainException("Số dư ví không đủ.");
        Balance -= amount;
    }
}