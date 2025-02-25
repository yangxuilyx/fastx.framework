using FastX.Data.DataFilters;
using SqlSugar;
using System.Linq.Expressions;
using FastX.DependencyInjection;
using FastX.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;
using FastX.Data.Entities;

namespace FastX.Data.SqlSugar;

public class XSugarContext
    : IXSugarContext, ITransientDependency
{
    public XSugarContext(ICurrentTenantAccessor currentTenantAccessor, ISqlSugarClient context, IServiceProvider serviceProvider)
    {
        _currentTenantAccessor = currentTenantAccessor;
        Context = context;
        ServiceProvider = serviceProvider;
    }

    public ISqlSugarClient Context { get; } 

    protected IServiceProvider ServiceProvider { get; } 

    public IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();

    public virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

    public virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

    private ICurrentTenantAccessor _currentTenantAccessor;

    protected virtual bool ShouldFilterEntity<TEntity>() where TEntity : IEntity
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            return true;
        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            return true;

        return false;
    }

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>() where TEntity : IEntity
    {
        Expression<Func<TEntity, bool>> expression = default!;

        if (IsSoftDeleteFilterEnabled && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> isDeleteFilter = t => ((ISoftDelete)t).IsDeleted == false;
            expression = expression == null ? isDeleteFilter : expression.And(isDeleteFilter);
        }
        if (IsMultiTenantFilterEnabled && typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)) && _currentTenantAccessor.Current != null)
        {
            Expression<Func<TEntity, bool>> multiTenantFilter = t => ((IMultiTenant)t).TenantId == _currentTenantAccessor.Current.TenantId;
            expression = expression == null ? multiTenantFilter : expression.And(multiTenantFilter);
        }

        return expression;
    }


    protected virtual void ConfigureGlobalFilters<TEntity>(SqlSugarClient context) where TEntity : IEntity
    {
        context.QueryFilter.Clear();

        if (ShouldFilterEntity<TEntity>())
        {
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
            {
                context.QueryFilter.AddTableFilter(filterExpression);
            }
        }
    }

    public ISqlSugarClient GetContext<TEntity>() where TEntity : IEntity
    {
        var context = Context.CopyNew();

        ConfigureGlobalFilters<TEntity>(context);

        return context;
    }
}