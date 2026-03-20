using EcommerceEnterprise.Domain.Entities.Products;
using EcommerceEnterprise.Domain.Entities.Users;
using EcommerceEnterprise.Domain.Entities.Warehouses;
using EcommerceEnterprise.Domain.Enums;
using EcommerceEnterprise.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EcommerceEnterprise.Infrastructure.Persistence.Seeders;

public class DataSeeder(
    AppDbContext context,
    ILogger<DataSeeder> logger)
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Bắt đầu seed data...");

        await SeedUsersAsync();
        await SeedCategoriesAsync();
        await SeedBranchesAsync();

        logger.LogInformation("Seed data hoàn thành.");
    }

    private async Task SeedUsersAsync()
    {
        if (await context.Users.AnyAsync()) return;

        var admin = User.Create(
            "admin@ecommerce.com",
            BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            "Super Admin",
            "0901234567",
            UserRole.SuperAdmin);

        var wallet = Domain.Entities.Users.Wallet.Create(admin.Id);

        context.Users.Add(admin);
        context.Wallets.Add(wallet);
        await context.SaveChangesAsync();

        logger.LogInformation("Đã tạo tài khoản admin.");
    }

    private async Task SeedCategoriesAsync()
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new() { Name = "Thời trang",  Slug = "thoi-trang",  SortOrder = 1 },
            new() { Name = "Điện tử",     Slug = "dien-tu",     SortOrder = 2 },
            new() { Name = "Nhà cửa",     Slug = "nha-cua",     SortOrder = 3 },
            new() { Name = "Sức khỏe",    Slug = "suc-khoe",    SortOrder = 4 },
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        logger.LogInformation("Đã tạo {Count} danh mục.", categories.Count);
    }

    private async Task SeedBranchesAsync()
    {
        if (await context.Branches.AnyAsync()) return;

        var branches = new List<Branch>
        {
            new() { Name = "Chi nhánh Hồ Chí Minh", Code = "HCM",
                    Province = "TP. Hồ Chí Minh", District = "Quận 1" },
            new() { Name = "Chi nhánh Hà Nội",      Code = "HN",
                    Province = "Hà Nội", District = "Hoàn Kiếm" },
            new() { Name = "Chi nhánh Đà Nẵng",     Code = "DN",
                    Province = "Đà Nẵng", District = "Hải Châu" },
        };

        context.Branches.AddRange(branches);
        await context.SaveChangesAsync();

        logger.LogInformation("Đã tạo {Count} chi nhánh.", branches.Count);
    }
}