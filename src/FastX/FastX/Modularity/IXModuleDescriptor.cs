using System.Reflection;

namespace FastX.Modularity;

public interface IXModuleDescriptor
{
    Type Type { get; }

    Assembly Assembly { get; }

    Assembly[] AllAssemblies { get; }

    IXModule Instance { get; }

    IReadOnlyList<IXModuleDescriptor> Dependencies { get; }
}