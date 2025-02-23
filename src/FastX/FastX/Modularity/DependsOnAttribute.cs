namespace FastX.Modularity;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute(params Type[]? dependedTypes) : Attribute, IDependedTypesProvider
{
    public Type[] DependedTypes { get; } = dependedTypes ?? Type.EmptyTypes;

    public Type[] GetDependedTypes() => DependedTypes;

}