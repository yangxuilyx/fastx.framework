using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FastX.Modularity;

/// <summary>
/// 模块系统
/// </summary>
public abstract class XModule : IXModule
{
    /// <summary>
    /// ConfigurationServiceAsync
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public virtual Task ConfigurationServiceAsync(IServiceCollection services)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// ConfigurationService
    /// </summary>
    /// <param name="services"></param>
    public virtual void ConfigurationService(IServiceCollection services) { }

    public virtual void PostConfigureServices(IServiceCollection services) { }

    public static bool IsXModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return
            typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IXModule).GetTypeInfo().IsAssignableFrom(type);
    }

    internal static void CheckXModuleType(Type moduleType)
    {
        if (!IsXModule(moduleType))
        {
            throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
        }
    }
}