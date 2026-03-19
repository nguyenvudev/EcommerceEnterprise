using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Warehouses;

public class Branch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public Guid? ManagerUserId { get; set; }
    public bool IsActive { get; set; } = true;
}