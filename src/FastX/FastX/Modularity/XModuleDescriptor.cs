using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FastX.Modularity;

public class XModuleDescriptor : IXModuleDescriptor
{
    public Type Type { get; }
    public Assembly Assembly { get; }
    public Assembly[] AllAssemblies { get; }
    public IXModule Instance { get; }
    public IReadOnlyList<IXModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
    private readonly List<IXModuleDescriptor> _dependencies;

    public XModuleDescriptor(
        [NotNull] Type type,
        [NotNull] IXModule instance,
        bool isLoadedAsPlugIn)
    {
        if (!type.GetTypeInfo().IsInstanceOfType(instance))
        {
            throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
        }

        Type = type;
        Assembly = type.Assembly;
        AllAssemblies = XModuleHelper.GetAllAssemblies(type);
        Instance = instance;

        _dependencies = new List<IXModuleDescriptor>();
    }

    public void AddDependency(IXModuleDescriptor descriptor)
    {
        _dependencies.AddIfNotContains(descriptor);
    }

    public override string ToString()
    {
        return $"[XModuleDescriptor {Type.FullName}]";
    }
}