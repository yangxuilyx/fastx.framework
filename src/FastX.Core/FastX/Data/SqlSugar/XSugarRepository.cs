using FastX.Data.Entities;
using FastX.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.Linq.Expressions;

namespace FastX.Data.SqlSugar;

public class XSugarRepository<TEntity>(IXSugarContext context, IOptions<XSugarOptions> options) : IRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    public IXSugarContext Context { get; } = context;

    protected XSugarOptions Options { get; } = options.Value;

    public async Task<TEntity?> GetAsync(object key)
    {
        if (key is Ulid ulidKey)
        {
            return await GetQueryable()
                .InSingleAsync(ulidKey.ToString());
        }
        return await GetQueryable()
            .InSingleAsync(key);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> where)
    {
        return await GetQueryable()
                   .Where(where)
                   .FirstAsync();
    }

    public async Task<List<TEntity>> GetListAsync()
    {
        return await GetQueryable()
            .ToListAsync();
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where)
    {
        return await GetQueryable()
                    .Where(where)
            .ToListAsync();
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        using var context = Context.GetContext<TEntity>();

        TryToSetCreateTime(entity);

        return await context.Insertable(entity)
            .AutoSetUlid()
            .ExecuteReturnEntityAsync();
    }

    public async Task InsertAsync(List<TEntity> entities, int pageSize = 0)
    {
        foreach (var entity in entities)
        {
            TryToSetCreateTime(entity);
        }

        using var context = Context.GetContext<TEntity>();

        if (pageSize == 0)
        {
            await context
                .Insertable(entities)
                .AutoSetUlid()
                .ExecuteCommandAsync();
        }
        else if (pageSize < 1000)
            await context.Insertable(entities).PageSize(pageSize)
                .ExecuteCommandAsync();
        else
            await context.Fastest<TEntity>().PageSize(pageSize).BulkCopyAsync(entities);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        using var context = Context.GetContext<TEntity>();

        TryToSetUpdateTime(entity);

        await context.Updateable(entity)
            .ExecuteCommandAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
    {
        using var context = Context.GetContext<TEntity>();

        TryToSetUpdateTime(entity);

        await context.Updateable(entity)
           .IgnoreColumns(ignoreColumns)
           .ExecuteCommandAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>? ignoreColumns, Expression<Func<TEntity, object>>? updateColumns)
    {
        using var context = Context.GetContext<TEntity>();

        TryToSetUpdateTime(entity);

        await context.Updateable(entity)
            .IgnoreColumns(ignoreAllNullColumns: true, ignoreAllDefaultValue: true)
            .IgnoreColumnsIF(ignoreColumns != null, ignoreColumns)
            .UpdateColumnsIF(updateColumns != null, updateColumns)
            .ExecuteCommandAsync();

        return entity;
    }

    public async Task<bool> UpdateAsync(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            TryToSetUpdateTime(entity);
        }

        using var context = Context.GetContext<TEntity>();

        var result = await context.Updateable(entities)
            .IgnoreColumns(ignoreAllNullColumns: true, ignoreAllDefaultValue: true)
            .ExecuteCommandAsync();

        return result > 0;
    }

    /// <summary>
    /// insert or update entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
    {
        TryToSetCreateTime(entity);

        using var context = Context.GetContext<TEntity>();

        var storage = await context.Storageable(entity).ToStorageAsync();

        if (await storage.AsUpdateable
                .ExecuteCommandAsync() > 0)
            return entity;

        return await storage.AsInsertable
            .AutoSetUlid()
            .ExecuteReturnEntityAsync();
    }


    public async Task<bool> DeleteAsync(TEntity entity)
    {
        using var context = Context.GetContext<TEntity>();

        if (entity is ISoftDelete && Context.IsSoftDeleteFilterEnabled)
        {
            return await context.Deleteable(entity)
                .IsLogic()
                .ExecuteCommandAsync() > 0;
        }

        return await context.Deleteable(entity)
            .ExecuteCommandAsync() > 0;
    }

    public async Task<bool> DeleteAsync(object key)
    {
        using var context = Context.GetContext<TEntity>();
        var entity = await GetAsync(key);
        if (entity == null)
            return true;

        return await DeleteAsync(entity);
    }

    internal ISugarQueryable<TEntity> GetQueryable()
    {
        using var context = Context.GetContext<TEntity>();
        return context
                .Queryable<TEntity>()
                .WithCacheIF(Options.EnableDataCache)
            ;
    }

    private static void TryToSetCreateTime(TEntity entity)
    {
        if (entity is IHasCreateTime createEntity)
        {
            if (createEntity.CreateTime == DateTime.MinValue)
            {
                createEntity.CreateTime = DateTime.Now;
            }
        }

        TryToSetUpdateTime(entity);
    }

    private static void TryToSetUpdateTime(TEntity entity)
    {
        if (entity is IHasUpdateTime updateEntity)
        {
            updateEntity.UpdateTime = DateTime.Now;
        }
    }
}