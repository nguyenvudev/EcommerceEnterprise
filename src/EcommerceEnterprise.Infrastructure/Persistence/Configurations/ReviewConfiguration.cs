using EcommerceEnterprise.Domain.Entities.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceEnterprise.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Content)
            .IsRequired()
            .HasMaxLength(2000);

        // Unique: 1 user chỉ review 1 lần cho 1 orderItem
        builder.HasIndex(r => new { r.UserId, r.OrderItemId })
            .IsUnique();
    }
}