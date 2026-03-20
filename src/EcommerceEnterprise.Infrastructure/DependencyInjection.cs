using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Domain.Interfaces;
using EcommerceEnterprise.Infrastructure.Caching;
using EcommerceEnterprise.Infrastructure.ExternalServices.Email;
using EcommerceEnterprise.Infrastructure.ExternalServices.Payment;
using EcommerceEnterprise.Infrastructure.Identity;
using EcommerceEnterprise.Infrastructure.Persistence;
using EcommerceEnterprise.Infrastructure.Persistence.Repositories;
using EcommerceEnterprise.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceEnterprise.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        // ── Database ──────────────────────────────────────────
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null)));

        // ── Repositories ──────────────────────────────────────
        services.AddScoped(
            typeof(IRepository<>),
            typeof(Repository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ── Identity ──────────────────────────────────────────
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        // ── Cache (Redis) ─────────────────────────────────────
        services.AddStackExchangeRedisCache(options =>
            options.Configuration =
                config.GetConnectionString("Redis"));

        services.AddScoped<ICacheService, RedisCacheService>();

        // ── External Services ─────────────────────────────────
        services.AddScoped<IPaymentGateway, VNPayService>();
        services.AddScoped<IEmailService, ConsoleEmailService>();

        // ── Seeders ───────────────────────────────────────────
        services.AddScoped<DataSeeder>();

        return services;
    }
}