namespace FastX.MultiTenancy;

public class BasicTenantInfo(Guid? tenantId, string? name = null)
{
    public Guid? TenantId { get; } = tenantId;

    public string? Name { get; } = name;
}
