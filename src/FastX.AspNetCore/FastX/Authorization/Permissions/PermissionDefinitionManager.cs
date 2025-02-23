using System.Collections.Immutable;
using FastX.Authorization.Permissions.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

namespace FastX.Authorization.Permissions
{
    /// <summary>
    /// PermissionDefinitionManager
    /// </summary>
    public class PermissionDefinitionManager : IPermissionDefinitionManager
    {
        /// <summary>
        /// PermissionGroupDefinitions
        /// </summary>
        protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;

        /// <summary>
        /// PermissionDefinitions
        /// </summary>
        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

        /// <summary>
        /// Options
        /// </summary>
        protected PermissionOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public PermissionDefinitionManager(IOptions<PermissionOptions> options, IServiceProvider serviceProvider)
        {
            Options = options.Value;
            _serviceProvider = serviceProvider;

            _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(
                CreatePermissionDefinitions,
                isThreadSafe: true
            );

            _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(
                CreatePermissionGroupDefinitions,
                isThreadSafe: true
            );
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionDefinition Get(string name)
        {
            var permission = GetOrNull(name);

            if (permission == null)
            {
                throw new Exception("权限不存在: " + name);
            }

            return permission;
        }

        /// <summary>
        /// GetOrNull
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionDefinition? GetOrNull(string name)
        {
            return PermissionDefinitions.TryGetValue(name, out var obj) ? obj : default;
        }

        /// <summary>
        /// GetPermissions
        /// </summary>
        /// <returns></returns>
        public virtual IReadOnlyList<PermissionDefinition> GetPermissions()
        {
            return PermissionDefinitions.Values.ToImmutableList();
        }

        /// <summary>
        /// GetGroups
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<PermissionGroupDefinition> GetGroups()
        {
            return PermissionGroupDefinitions.Values.ToImmutableList();
        }

        /// <summary>
        /// CreatePermissionDefinitions
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
        {
            var permissions = new Dictionary<string, PermissionDefinition>();

            foreach (var groupDefinition in PermissionGroupDefinitions.Values)
            {
                foreach (var permission in groupDefinition.Permissions)
                {
                    AddPermissionToDictionaryRecursively(permissions, permission);
                }
            }

            return permissions;
        }

        /// <summary>
        /// AddPermissionToDictionaryRecursively
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="permission"></param>
        protected virtual void AddPermissionToDictionaryRecursively(
            Dictionary<string, PermissionDefinition> permissions,
            PermissionDefinition permission)
        {
            if (permissions.ContainsKey(permission.Name))
            {
                throw new Exception("权限定义重复: " + permission.Name);
            }

            permissions[permission.Name] = permission;

            foreach (var child in permission.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, child);
            }
        }

        /// <summary>
        /// CreatePermissionGroupDefinitions
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = new PermissionDefinitionContext(scope.ServiceProvider);

            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider?.Define(context);
            }

            return context.Groups;
        }
    }
}