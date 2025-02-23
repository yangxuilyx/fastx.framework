using FastX.Data.SqlSugar;
using FastX.Guids;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FastX.Data.SqlSugar.DataAnnotations;

namespace Microsoft.Extensions.DependencyInjection;

public static class SugarServiceRegistrationCollectionExtensions
{
    public static XSugarBuilder AddXSqlSugar(this IServiceCollection services, [NotNull] Action<XSugarRegistrationOptions> optionsAction)
    {
        var options = new XSugarRegistrationOptions();
        optionsAction.Invoke(options);

        if (options.DbType is DbType.PostgreSQL or DbType.MySql)
            services.Configure<SequentialGuidGeneratorOptions>(t =>
                t.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString);
        else if (options.DbType is DbType.Oracle or DbType.Dm)
            services.Configure<SequentialGuidGeneratorOptions>(t =>
                t.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary);
        else services.Configure<SequentialGuidGeneratorOptions>(t =>
            t.DefaultSequentialGuidType = SequentialGuidType.SequentialAtEnd);

        var sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = options.DbType,
            ConnectionString = options.ConnectionString,
            IsAutoCloseConnection = options.IsAutoCloseConnection,
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityService = (propertyInfo, columnInfo) =>
                {
                    var attributes = propertyInfo.GetCustomAttributes(true);

                    // default primary key 
                    if (columnInfo.DbColumnName == $"{columnInfo.EntityName}Id")
                        columnInfo.IsPrimarykey = true;
                    if (attributes.Any(t => t is KeyAttribute))
                        columnInfo.IsPrimarykey = true;

                    if (propertyInfo.PropertyType == typeof(Ulid) || propertyInfo.PropertyType == typeof(Ulid?))
                    {
                        columnInfo.Length = 26;
                        columnInfo.SqlParameterDbType = typeof(UlidDataConverter);
                    }

                    if (columnInfo.IsPrimarykey == false && new NullabilityInfoContext()
                            .Create(propertyInfo).WriteState is NullabilityState.Nullable)
                    {
                        columnInfo.IsNullable = true;
                    }
                },
                EntityNameService = ((type, entityInfo) =>
                {
                    var customAttributes = type.GetCustomAttributes(true);

                    if (customAttributes.FirstOrDefault(t => t is XSugarTableAttribute) is XSugarTableAttribute tableAttribute)
                    {
                        var tableName = $"{tableAttribute.Prefix}_" + (!tableAttribute.TableName.IsNullOrEmpty() ? tableAttribute.TableName : entityInfo.DbTableName);
                        entityInfo.DbTableName = tableName;
                    }
                })
            },

        }, db =>
        {
            db.Aop.OnLogExecuting = (sql, parameters) =>
            {
                Console.WriteLine("------sql begin ------");
                Console.WriteLine(sql);
                Console.WriteLine("------sql end ------");

            };
        });

        services.AddTransient<ISqlSugarClient>(t => sqlSugarClient);

        StaticConfig.CustomGuidFunc = () =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IGuidGenerator>().Create();
        };

        var fastXSqlSugarBuilder = new XSugarBuilder(sqlSugarClient, services);
        services.AddSingleton(fastXSqlSugarBuilder);
        return fastXSqlSugarBuilder;
    }

}