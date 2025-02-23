using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FastX.Internal;
using FastX.Modularity;

namespace FastX;

public class XApplicationBase : IXApplication
{
    [NotNull]
    public Type StartupModuleType { get; }
    public IServiceCollection Services { get; private set; } = default!;
    public IServiceProvider ServiceProvider { get; }

    public IReadOnlyList<IXModuleDescriptor> Modules { get; }

    private bool _configuredServices;

    public XApplicationBase(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services
        )
    {
        StartupModuleType = startupModuleType;
        Services = services;

        services.AddSingleton<IXApplication>(this);

        services.AddCoreServices();

        var serviceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        ServiceProvider = serviceScope.ServiceProvider;

        Modules = LoadModules(services);

        ConfigureServices();
    }

    protected void ConfigureServices()
    {
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.ConfigurationService(Services);
            }
            catch (Exception ex)
            {
                throw new XException($"An error occurred during ConfigurationService  phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }
    }

    public async Task ConfigureServicesAsync()
    {
        CheckMultipleConfigureServices();

        var assemblies = new HashSet<Assembly>();

        foreach (var module in Modules)
        {
            foreach (var assembly in module.AllAssemblies)
            {
                if (assemblies.Add(assembly))
                {
                    Services.AddAssembly(assembly);
                }
            }
        }

        foreach (var module in Modules)
        {
            try
            {
                await module.Instance.ConfigurationServiceAsync(Services);
            }
            catch (Exception ex)
            {
                throw new XException($"An error occurred during ConfigurationServiceAsync  phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PostConfigureServices(Services);
            }
            catch (Exception ex)
            {
                throw new XException($"An error occurred during PostConfigureServices  phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        _configuredServices = true;
    }

    private void CheckMultipleConfigureServices()
    {
        if (_configuredServices)
        {
            throw new XException("Services have already been configured!");
        }
    }

    protected virtual IReadOnlyList<IXModuleDescriptor> LoadModules(IServiceCollection services)
    {
        return services.GetSingletonInstance<IModuleLoader>()
            .LoadModules(services, StartupModuleType);
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}