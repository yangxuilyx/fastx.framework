namespace FastX.MultiTenancy;

public interface ICurrentTenantAccessor
{
    BasicTenantInfo? Current { get; set; }
}