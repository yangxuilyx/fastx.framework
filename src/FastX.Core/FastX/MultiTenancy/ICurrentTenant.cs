namespace FastX.MultiTenancy;

public interface ICurrentTenant
{
    bool IsAvailable { get; }

    Guid? TenantId { get; }

    string? Name { get; }

    IDisposable Change(Guid? tenantId, string? name = null);
}