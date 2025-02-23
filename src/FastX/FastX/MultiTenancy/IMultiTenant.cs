namespace FastX.MultiTenancy;

public interface IMultiTenant
{
    /// <summary>
    /// TenantId
    /// </summary>
    Guid? TenantId { get; set; }
}