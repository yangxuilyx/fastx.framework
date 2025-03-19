using FastX.Data.Entities;
using System.Linq.Expressions;

namespace FastX.Data.Repository;

public interface IRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    /// <summary>
    /// get entity by keys
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(object key);

    /// <summary>
    /// get entity by the given predicate
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// get entity list 
    /// </summary>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync();

    /// <summary>
    /// get entity list by the given predicate
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// insert a new entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> InsertAsync(TEntity entity);

    /// <summary>
    /// bulk insert entities
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="pageSize">when pageSize=0 call Insertable method and auto general ulid primaryKey, when pageSize &lt; 1000 call PageSize method,else call BulkCopy method. default : 0</param>
    /// <returns></returns>
    Task InsertAsync(List<TEntity> entities, int pageSize = 0);

    /// <summary>
    /// update  entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// update  entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="ignoreColumns"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

    /// <summary>
    /// update  entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="ignoreColumns"></param>
    /// <param name="updateColumns"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>? ignoreColumns, Expression<Func<TEntity, object>>? updateColumns);

    /// <summary>
    /// bulk update entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(List<TEntity> entities);

    /// <summary>
    /// insert or update entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> InsertOrUpdateAsync(TEntity entity);

    /// <summary>
    /// delete entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity entity);

    /// <summary>
    /// delete entity by keys
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(object key);
}