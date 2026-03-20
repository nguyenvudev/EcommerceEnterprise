using EcommerceEnterprise.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceEnterprise.Infrastructure.Persistence.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.SKU)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(v => v.SKU)
            .IsUnique();

        builder.Property(v => v.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(v => v.CompareAtPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(v => v.Weight)
            .HasColumnType("decimal(10,3)");

        builder.Property(v => v.AttributesJson)
            .HasMaxLength(1000);
    }
}