using EcommerceEnterprise.Domain.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceEnterprise.Infrastructure.Persistence.Configurations;

public class WarehouseStockConfiguration
    : IEntityTypeConfiguration<WarehouseStock>
{
    public void Configure(EntityTypeBuilder<WarehouseStock> builder)
    {
        builder.HasKey(s => s.Id);

        // Unique: mỗi kho + variant chỉ có 1 record
        builder.HasIndex(s => new { s.WarehouseId, s.VariantId })
            .IsUnique();
    }
}