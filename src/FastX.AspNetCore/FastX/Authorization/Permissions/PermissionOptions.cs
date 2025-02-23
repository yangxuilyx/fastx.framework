using FastX.Authorization.Permissions.Abstractions;
using FastX.Collections;

namespace FastX.Authorization.Permissions
{
    /// <summary>
    /// DefinitionProviders
    /// </summary>
    public class PermissionOptions
    {
        /// <summary>
        /// DefinitionProviders
        /// </summary>
        public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

        /// <summary>
        /// PermissionOptions
        /// </summary>
        public PermissionOptions()
        {
            DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
        }
    }
}