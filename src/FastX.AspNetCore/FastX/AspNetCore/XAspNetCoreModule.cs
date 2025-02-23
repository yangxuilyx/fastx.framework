using Cysharp.Serialization.Json;
using FastX.AspNetCore.Conventions;
using FastX.Authorization;
using FastX.Authorization.Permissions;
using FastX.Authorization.Permissions.Abstractions;
using FastX.Modularity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace FastX.AspNetCore;

[DependsOn(typeof(XCoreModule))]
public class XAspNetCoreModule : XModule
{
    public override void ConfigurationService(IServiceCollection services)
    {
        services.AddXAuthorization();

        services.AddControllers(options =>
            {
                options.Filters.Add<AutoValidateModelStateFilter>();
            })
        .AddControllersAsServices()
        .AddJsonOptions(t =>
        {
            t.JsonSerializerOptions.Converters.Add(new UlidJsonConverter());
            t.JsonSerializerOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        });

        services.AddAutoMapper(options =>
        {
        });
        services.AddHttpContextAccessor();

        var applicationPartManager = services.GetSingletonInstance<ApplicationPartManager>();
        applicationPartManager.FeatureProviders.Add(new XConventionalControllerFeatureProvider());

        services.Configure<MvcOptions>(options =>
        {
            options.Conventions.Add(new XServiceConventionWrapper(services));
        });

    }

    /// <summary>PostConfigureServices</summary>
    /// <param name="services"></param>
    public override void PostConfigureServices(IServiceCollection services)
    {
        var definitionProviders = services
            .Where(p => typeof(IPermissionDefinitionProvider).IsAssignableFrom(p.ImplementationType))
            .Select(p => p.ImplementationType);

        services.Configure<PermissionOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}