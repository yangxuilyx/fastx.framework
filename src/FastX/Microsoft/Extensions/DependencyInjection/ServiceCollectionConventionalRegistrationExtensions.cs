using FastX.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionConventionalRegistrationExtensions
{
    private static IConventionalRegistrar GetOrCreateConventionalRegistrar(this IServiceCollection services)
    {
        var conventionalRegistrar = services.GetSingletonInstanceOrNull<IConventionalRegistrar>();
        if (conventionalRegistrar == null)
        {
            conventionalRegistrar = new ConventionalRegistrar();
            services.AddSingleton<IConventionalRegistrar>(conventionalRegistrar);
        }

        return conventionalRegistrar;
    }

    public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
    {
        return services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
    }

    public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
    {
        var registrar = services.GetOrCreateConventionalRegistrar();
        registrar.AddAssembly(services, assembly);

        return services;
    }
}