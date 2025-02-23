using FastX.Data.Entities;
using SqlSugar;
using System.Linq.Expressions;

namespace FastX.Data.SqlSugar;

public interface IXSugarContext
{
    bool IsSoftDeleteFilterEnabled { get; }

    ISqlSugarClient GetContext<TEntity>() where TEntity : IEntity;
}