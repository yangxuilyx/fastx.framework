using System.Diagnostics.CodeAnalysis;
using FastX.DependencyInjection;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// PermissionDefinitionContext
    /// </summary>
    public class PermissionDefinitionContext : IPermissionDefinitionContext, ITransientDependency
    {
        /// <summary>
        /// ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Groups
        /// </summary>
        public Dictionary<string, PermissionGroupDefinition> Groups { get; }

        /// <summary>
        /// PermissionDefinitionContext
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PermissionDefinitionContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Groups = new Dictionary<string, PermissionGroupDefinition>();
        }

        /// <summary>
        /// AddGroup
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public virtual PermissionGroupDefinition AddGroup(
            string name,
            string no,
            string displayName = null,
            string description = null
           )
        {
            if (Groups.ContainsKey(name))
            {
                throw new Exception($"权限组已存在: {name}");
            }

            return Groups[name] = new PermissionGroupDefinition(name, no, displayName, description);
        }

        /// <summary>
        /// GetGroup
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionGroupDefinition GetGroup([NotNull] string name)
        {
            var group = GetGroupOrNull(name);

            if (group == null)
            {
                throw new Exception($"权限组不存在: {name}");
            }

            return group;
        }

        /// <summary>
        /// GetGroupOrNull
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionGroupDefinition GetGroupOrNull([NotNull] string name)
        {
            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        /// <summary>
        /// RemoveGroup
        /// </summary>
        /// <param name="name"></param>
        public virtual void RemoveGroup(string name)
        {

            if (!Groups.ContainsKey(name))
            {
                throw new Exception($"权限组不存在: {name}");
            }

            Groups.Remove(name);
        }

        /// <summary>
        /// GetPermissionOrNull
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionDefinition GetPermissionOrNull([NotNull] string name)
        {
            foreach (var groupDefinition in Groups.Values)
            {
                var permissionDefinition = groupDefinition.GetPermissionOrNull(name);

                if (permissionDefinition != null)
                {
                    return permissionDefinition;
                }
            }

            return null;
        }
    }
}