using SqlSugar;

namespace FastX.Data.Repository;

public static class RepositoryExtensions
{
    //public static Task<List<TResult>> SqlQuery<TResult>(
    //    this IRepository repository,
    //    string sql) where TResult : class, new()
    //{
    //    var sugarQueryable = repository.Context.SqlQueryable<TResult>(sql);

    //    return sugarQueryable.ToListAsync();
    //}

    //public static Task<int> ExecuteCommandAsync(
    //    this IRepository repository,
    //    string sql,
    //    params SugarParameter[] parameters
    //)
    //{
    //    return repository.Context.Ado.ExecuteCommandAsync(sql, parameters);
    //}


}