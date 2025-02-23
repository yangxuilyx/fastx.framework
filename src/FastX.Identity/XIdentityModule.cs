using FastX.Identity.Core.Identity.Users;
using FastX.Modularity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FastX.Identity;

public class XIdentityModule : XModule
{
    /// <summary>
    /// ConfigurationService
    /// </summary>
    /// <param name="services"></param>
    public override void ConfigurationService(IServiceCollection services)
    {
        services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddAutoMapper(typeof(XIdentityModule).Assembly);
    }

    /// <summary>
    /// PostConfigureServices
    /// </summary>
    /// <param name="services"></param>
    public override void PostConfigureServices(IServiceCollection services)
    {
        services.GetSingletonInstance<XSugarBuilder>().CodeFirstInitTables(typeof(XIdentityModule).Assembly);
    }
}