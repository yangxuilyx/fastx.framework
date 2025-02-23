using FastX.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FastX.DependencyInjection;

public class ConventionalRegistrar : IConventionalRegistrar
{
    public void AddAssembly(IServiceCollection services, Assembly assembly)
    {
        var types = AssemblyHelper
            .GetAllTypes(assembly)
            .Where(t => t != null &&
                        t.IsClass
                        && !t.IsAbstract &&
                        !t.IsGenericType).ToArray();

        AddTypes(services, types);
    }

    public void AddTypes(IServiceCollection services, params Type[] types)
    {
        foreach (var type in types)
        {
            AddType(services, type);
        }
    }

    public void AddType(IServiceCollection services, Type type)
    {
        var lifeTime = GetLifeTimeOrNull(type);
        if (lifeTime == null)
            return;

        foreach (var exposingServiceType in GetDefaultServices(type))
        {
            var serviceDescriptor = CreateServiceDescriptor(type, exposingServiceType, lifeTime.Value);
            services.Add(serviceDescriptor);
        }
    }

    private static List<Type> GetDefaultServices(Type type)
    {
        var serviceTypes = new List<Type>
        {
            type
        };
        foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
        {
            var interfaceName = interfaceType.Name;
            if (interfaceType.IsGenericType)
            {
                interfaceName = interfaceType.Name.Left(interfaceType.Name.IndexOf('`'));
            }

            if (interfaceName.StartsWith("I"))
            {
                interfaceName = interfaceName.Right(interfaceName.Length - 1);
            }

            if (type.Name.EndsWith(interfaceName))
            {
                serviceTypes.Add(interfaceType);
            }
        }

        return serviceTypes;
    }

    protected virtual ServiceDescriptor CreateServiceDescriptor(
        Type implementationType,
        Type exposingServiceType,
        ServiceLifetime lifeTime
        )
    {
        return ServiceDescriptor.Describe(exposingServiceType,
            implementationType,
            lifeTime);
    }


    protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type)
    {
        return GetServiceLifetimeFromClassHierarchy(type) ?? GetDefaultLifeTimeOrNull(type);
    }

    protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
    {
        if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Transient;
        }

        if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Singleton;
        }

        if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Scoped;
        }

        return null;
    }

    protected virtual ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
    {
        return null;
    }
}