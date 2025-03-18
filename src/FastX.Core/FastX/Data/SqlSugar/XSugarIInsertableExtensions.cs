using System.Diagnostics.CodeAnalysis;
using FastX.Data.Entities;
using SqlSugar;

namespace FastX.Data.SqlSugar;

public static class XSugarIInsertableExtensions
{
    public static IInsertable<TEntity> AutoSetUlid<TEntity>([NotNull] this IInsertable<TEntity> insertable) where TEntity : class, IEntity, new()
    {
        var primaryKeys = GetPrimaryKeys(insertable);
        foreach (var primaryKey in primaryKeys)
        {
            var columnInfos = insertable.InsertBuilder.DbColumnInfoList.Where(t => t.DbColumnName == primaryKey).ToList();
            foreach (var columnInfo in columnInfos)
            {
                if (columnInfo != null && columnInfo.PropertyType == typeof(Ulid) &&
                    columnInfo.Value.ToString().IsNullOrEmptyUlid())
                {
                    var columnInfoValue = Ulid.NewUlid();
                    columnInfo.Value = columnInfoValue;

                    if (insertable is not InsertableProvider<TEntity> insertableProvider)
                        continue;
                    if (insertableProvider.InsertObjs.Length != 1)
                        continue;

                    var primaryInfoColumn = insertable.InsertBuilder.EntityInfo.Columns.First(t => t.DbColumnName == primaryKey);
                    primaryInfoColumn.PropertyInfo.SetValue(insertableProvider.InsertObjs.First(), columnInfoValue);
                }
            }
        }

        return insertable;
    }

    private static List<string> GetPrimaryKeys<TEntity>(IInsertable<TEntity> insertable) where TEntity : class, IEntity, new()
    {
        var insertBuilder = insertable.InsertBuilder;
        if (insertBuilder.Context.IsSystemTablesConfig)
        {
            return insertBuilder.Context.DbMaintenance.GetPrimaries(insertBuilder.Context.EntityMaintenance.GetTableName(insertBuilder.EntityInfo.EntityName));
        }
        return (from it in insertBuilder.EntityInfo.Columns
                where it.IsPrimarykey
                select it.DbColumnName).ToList();
    }
}