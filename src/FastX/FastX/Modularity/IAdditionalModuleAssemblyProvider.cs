using System.Reflection;

namespace FastX.Modularity;

public interface IAdditionalModuleAssemblyProvider
{
    Assembly[] GetAssemblies();
}