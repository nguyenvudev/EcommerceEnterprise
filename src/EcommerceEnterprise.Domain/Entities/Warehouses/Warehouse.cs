using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Warehouses;

public class Warehouse : BaseEntity
{
    public Guid BranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}