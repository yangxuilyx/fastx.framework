using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FastX.Data.Entities;
using SqlSugar;

namespace Microsoft.Extensions.DependencyInjection;

public class XSugarBuilder(ISqlSugarClient sqlSugarClient, IServiceCollection services)
{
    /// <summary>
    /// SqlSugarClient
    /// </summary>
    public ISqlSugarClient SqlSugarClient { get; set; } = sqlSugarClient;

    /// <summary>
    /// Services
    /// </summary>
    public IServiceCollection Services { get; set; } = services;

    /// <summary>
    /// CodeFirstInitTables
    /// </summary>
    /// <param name="assembly"></param>
    public void CodeFirstInitTables([NotNull] Assembly assembly)
    {
        var entityTypes = assembly.GetTypes().Where(t => typeof(IEntity).IsAssignableFrom(t)).ToArray();
        SqlSugarClient.CodeFirst.InitTables(entityTypes);
    }
}