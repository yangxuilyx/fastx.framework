using System.Diagnostics.CodeAnalysis;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// IPermissionDefinitionContext
    /// </summary>
    public interface IPermissionDefinitionContext
    {
        /// <summary>
        /// ServiceProvider
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets a pre-defined permission group.
        /// </summary>
        /// <param name="name">Name of the group</param>
        /// <returns></returns>
        PermissionGroupDefinition GetGroup(string name);

        /// <summary>
        /// Tries to get a pre-defined permission group.
        /// Returns null if can not find the given group.
        /// </summary>
        /// <param name="name">Name of the group</param>
        /// <returns></returns>
        PermissionGroupDefinition GetGroupOrNull(string name);

        /// <summary>
        /// AddGroup
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        PermissionGroupDefinition AddGroup(
            string name,
            string no,
            string displayName = null,
            string description = null);

        /// <summary>
        /// RemoveGroup
        /// </summary>
        /// <param name="name"></param>
        void RemoveGroup(string name);

        /// <summary>
        /// GetPermissionOrNull
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PermissionDefinition GetPermissionOrNull([NotNull] string name);
    }
}