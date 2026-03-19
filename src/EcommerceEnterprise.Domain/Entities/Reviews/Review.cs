using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Exceptions;

namespace EcommerceEnterprise.Domain.Entities.Reviews;

public class Review : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderItemId { get; set; }
    public byte Rating { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImagesJson { get; set; }
    public bool IsVerifiedPurchase { get; set; } = true;
    public bool IsFlagged { get; set; } = false;

    public static Review Create(Guid productId, Guid userId,
        Guid orderItemId, byte rating, string content)
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("Rating phải từ 1 đến 5.");

        return new Review
        {
            ProductId = productId,
            UserId = userId,
            OrderItemId = orderItemId,
            Rating = rating,
            Content = content
        };
    }
}