using EcommerceEnterprise.Domain.Entities.Promotions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceEnterprise.Infrastructure.Persistence.Configurations;

public class FlashSaleItemConfiguration : IEntityTypeConfiguration<FlashSaleItem>
{
    public void Configure(EntityTypeBuilder<FlashSaleItem> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.SalePrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(f => f.OriginalPrice)
            .HasColumnType("decimal(18,2)");
    }
}