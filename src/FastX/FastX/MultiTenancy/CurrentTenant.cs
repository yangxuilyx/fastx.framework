using FastX.DependencyInjection;

namespace FastX.MultiTenancy;

public class CurrentTenant : ICurrentTenant, ITransientDependency
{
    /// <summary>
    /// CurrentTenant
    /// </summary>
    /// <param name="currentTenantAccessor"></param>
    public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor) => _currentTenantAccessor = currentTenantAccessor;

    /// <inheritdoc />
    public bool IsAvailable => TenantId.HasValue;

    /// <inheritdoc />
    public Guid? TenantId => _currentTenantAccessor.Current?.TenantId;

    /// <inheritdoc />
    public string? Name => _currentTenantAccessor.Current?.Name;

    private readonly ICurrentTenantAccessor _currentTenantAccessor;

    /// <inheritdoc />
    public IDisposable Change(Guid? tenantId, string? name = null)
    {
        return SetCurrent(tenantId, name);
    }

    private IDisposable SetCurrent(Guid? tenantId, string? name = null)
    {
        var parentScope = _currentTenantAccessor.Current;
        _currentTenantAccessor.Current = new BasicTenantInfo(tenantId, name);

        return new DisposeAction<ValueTuple<ICurrentTenantAccessor, BasicTenantInfo?>>(static state =>
        {
            var (currentTenantAccessor, parentScope) = state;
            currentTenantAccessor.Current = parentScope;

        }, (_currentTenantAccessor, parentScope));
    }
}