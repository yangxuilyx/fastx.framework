using FastX.DependencyInjection;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// PermissionDefinitionProvider
    /// </summary>
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
    {
        /// <summary>
        /// Define
        /// </summary>
        /// <param name="context"></param>
        public abstract void Define(IPermissionDefinitionContext context);

    }
}