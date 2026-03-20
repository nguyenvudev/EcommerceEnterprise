using EcommerceEnterprise.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceEnterprise.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Đăng ký MediatR — tự tìm tất cả Handler trong assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        // Đăng ký AutoMapper
        services.AddAutoMapper(assembly);

        // Đăng ký FluentValidation — tự tìm tất cả Validator
        services.AddValidatorsFromAssembly(assembly);

        // Đăng ký Pipeline Behaviors — chạy theo thứ tự
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}