using FastX.Data.Entities;
using FastX.Data.Repository;
using SqlSugar;

namespace FastX.Data.SqlSugar;

public static class XSugarRepositoryExtensions
{
    public static Task<List<TResult>> SqlQuery<TResult, TEntity>(
        this IRepository repository,
        string sql) where TResult : class, new() where TEntity : class, IEntity, new()
    {
        if (repository is XSugarRepository<TEntity> xSugarRepository)
        {
            var sugarQueryable = xSugarRepository.Context.GetContext<TEntity>().SqlQueryable<TResult>(sql);
            return sugarQueryable.ToListAsync();
        }

        throw new NotImplementedException("can not get XSugarRepository with other repository");
    }

    public static Task<int> ExecuteCommandAsync<TEntity>(
        this IRepository repository,
        string sql,
        params SugarParameter[] parameters
    ) where TEntity : class, IEntity, new()
    {
        if (repository is XSugarRepository<TEntity> xSugarRepository)
        {
            return xSugarRepository.Context.GetContext<TEntity>().Ado.ExecuteCommandAsync(sql, parameters);
        }

        throw new NotImplementedException("can not get XSugarRepository with other repository");
    }

    /// <summary>
    /// get queryable
    /// </summary>
    /// <returns></returns>
    public static ISugarQueryable<TEntity> GetQueryable<TEntity>(this IRepository repository) where TEntity : class, IEntity, new()
    {
        if (repository is XSugarRepository<TEntity> xSugarRepository)
            return xSugarRepository.GetQueryable();

        throw new NotImplementedException("can not get XSugarRepository with other repository");
    }
}