using FastX.MultiTenancy;

namespace FastX.Data.Entities;

public static class EntityHelper
{
    public static void TrySetTenantId(IEntity entity)
    {
        if (entity is not IMultiTenant multiTenantEntity)
        {
            return;
        }

        var tenantId = AsyncLocalCurrentTenantAccessor.Instance.Current?.TenantId;
        if (tenantId == multiTenantEntity.TenantId)
            return;

        multiTenantEntity.TenantId = tenantId;
    }
}