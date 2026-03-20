using EcommerceEnterprise.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceEnterprise.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Method)
            .HasConversion<byte>();

        builder.Property(p => p.Status)
            .HasConversion<byte>();

        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.TxnRef)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.TxnRef);

        builder.Property(p => p.GatewayTransactionId)
            .HasMaxLength(200);
    }
}