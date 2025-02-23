using Autofac;
using FastX.Autofac;

namespace Microsoft.Extensions.Hosting;

public static class AutofacHostBuilderExtensions
{
    public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
    {
        var containerBuilder = new ContainerBuilder();

        return hostBuilder.ConfigureServices((_, services) =>
            {
            })
            .UseServiceProviderFactory(new XAutofacServiceProviderFactory(containerBuilder));
    }
}
