using EcommerceEnterprise.Domain.Interfaces;
using EcommerceEnterprise.Infrastructure.Persistence;

namespace EcommerceEnterprise.Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);

    public void Dispose()
        => context.Dispose();
}