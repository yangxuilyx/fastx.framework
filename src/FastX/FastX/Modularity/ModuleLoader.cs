using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace FastX.Modularity;

public class ModuleLoader : IModuleLoader
{
    public IXModuleDescriptor[] LoadModules([NotNull] IServiceCollection services, [NotNull] Type startupModuleType)
    {
        Check.NotNull(services, nameof(services));
        Check.NotNull(startupModuleType, nameof(startupModuleType));

        var modules = GetDescriptors(services, startupModuleType);
        modules = SortByDependency(modules, startupModuleType);

        return modules.ToArray();
    }

    private List<IXModuleDescriptor> GetDescriptors(
        IServiceCollection services,
        Type startupModuleType)
    {
        var logger = services.GetRequiredService<ILogger<IXApplication>>();

        var modules = new List<XModuleDescriptor>();

        foreach (var moduleType in XModuleHelper.FindAllModuleTypes(startupModuleType, logger))
        {
            modules.Add(CreateModuleDescriptor(services, moduleType));
        }

        SetDependencies(modules);

        return modules.Cast<IXModuleDescriptor>().ToList();
    }


    protected virtual List<IXModuleDescriptor> SortByDependency(List<IXModuleDescriptor> modules, Type startupModuleType)
    {
        var sortedModules = modules.SortByDependencies(m => m.Dependencies);
        sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return sortedModules;
    }

    protected virtual void SetDependencies(List<XModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            SetDependencies(modules, module);
        }
    }


    protected virtual void SetDependencies(List<XModuleDescriptor> modules, XModuleDescriptor module)
    {
        foreach (var dependedModuleType in XModuleHelper.FindDependedModuleTypes(module.Type))
        {
            var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
            if (dependedModule == null)
            {
                throw new XException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
            }

            module.AddDependency(dependedModule);
        }
    }

    protected virtual XModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
    {
        return new XModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
    }

    protected virtual IXModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
    {
        var module = (IXModule)Activator.CreateInstance(moduleType)!;
        services.AddSingleton(moduleType, module);
        return module;
    }
}