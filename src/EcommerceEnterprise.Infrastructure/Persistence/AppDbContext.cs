using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Entities.Payments;
using EcommerceEnterprise.Domain.Entities.Products;
using EcommerceEnterprise.Domain.Entities.Promotions;
using EcommerceEnterprise.Domain.Entities.Reviews;
using EcommerceEnterprise.Domain.Entities.Users;
using EcommerceEnterprise.Domain.Entities.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceEnterprise.Infrastructure.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IMediator mediator) : DbContext(options)
{
    // ── DbSets ────────────────────────────────────────────────
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseStock> WarehouseStocks => Set<WarehouseStock>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<FlashSale> FlashSales => Set<FlashSale>();
    public DbSet<FlashSaleItem> FlashSaleItems => Set<FlashSaleItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tự động load tất cả Configuration trong assembly
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        // Global Query Filter — tự động lọc IsDeleted = false
        modelBuilder.Entity<User>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Product>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Order>()
            .HasQueryFilter(e => !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken ct = default)
    {
        // Tự động cập nhật UpdatedAt
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        var result = await base.SaveChangesAsync(ct);

        // Dispatch domain events sau khi lưu DB
        var aggregates = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.DomainEvents)
                await mediator.Publish(domainEvent, ct);

            aggregate.ClearDomainEvents();
        }

        return result;
    }
}