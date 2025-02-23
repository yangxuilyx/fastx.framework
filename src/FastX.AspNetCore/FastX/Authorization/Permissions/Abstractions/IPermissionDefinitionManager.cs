using System.Diagnostics.CodeAnalysis;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// IPermissionDefinitionManager
    /// </summary>
    public interface IPermissionDefinitionManager
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PermissionDefinition Get([NotNull] string name);

        /// <summary>
        /// GetOrNull
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PermissionDefinition? GetOrNull([NotNull] string name);

        /// <summary>
        /// GetPermissions
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<PermissionDefinition> GetPermissions();

        /// <summary>
        /// GetGroups
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<PermissionGroupDefinition> GetGroups();
    }
}