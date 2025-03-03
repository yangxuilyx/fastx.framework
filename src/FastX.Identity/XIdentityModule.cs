using FastX.Identity.Core.Identity.Users;
using FastX.Modularity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenIddict.Abstractions;

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

        //services.AddOpenIddict()
        //    .AddServer(options =>
        //    {
        //        options.SetTokenEndpointUris("connect/token");

        //        // Enable the client credentials flow.
        //        options.AllowClientCredentialsFlow();

        //        // Register the signing and encryption credentials.
        //        options.AddDevelopmentEncryptionCertificate()
        //            .AddDevelopmentSigningCertificate();

        //        // Register the ASP.NET Core host and configure the ASP.NET Core options.
        //        options.UseAspNetCore()
        //            .EnableTokenEndpointPassthrough();
        //    })
        //    .AddCore(options =>
        //    {
        //    })
        //    ;
    }

    /// <summary>
    /// PostConfigureServices
    /// </summary>
    /// <param name="services"></param>
    public override void PostConfigureServices(IServiceCollection services)
    {
        services.GetSingletonInstance<XSugarBuilder>().InitTables(typeof(XIdentityModule).Assembly);

        var manager = services.BuildServiceProvider().CreateScope().ServiceProvider
            .GetRequiredService<IOpenIddictApplicationManager>();

        //manager.CreateAsync(new OpenIddictApplicationDescriptor()
        //{
        //    ClientId = "service-worker",
        //    ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
        //    Permissions =
        //    {
        //        OpenIddictConstants.Permissions.Endpoints.Token,
        //        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials
        //    }
        //});
    }
}