using FastX.AspNetCore;
using FastX.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace FastX.CodeGenerate;

[DependsOn(typeof(XAspNetCoreModule))]
public class XCodeGenerateModule : XModule
{
    /// <summary>PostConfigureServices</summary>
    /// <param name="services"></param>
    public override void PostConfigureServices(IServiceCollection services)
    {
        services.GetSingletonInstance<XSugarBuilder>().CodeFirstInitTables(typeof(XCodeGenerateModule).Assembly);
    }
}