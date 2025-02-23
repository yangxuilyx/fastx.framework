using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// PermissionGroupDefinition
    /// </summary>
    public class PermissionGroupDefinition
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 权限组说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();
        private readonly List<PermissionDefinition> _permissions;

        /// <summary>
        /// PermissionGroupDefinition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        protected internal PermissionGroupDefinition(
            string name,
            string no,
            string displayName = null,
            string description = null
            )
        {
            Name = name;
            No = no;
            DisplayName = displayName;
            Description = description;

            _permissions = new List<PermissionDefinition>();
        }

        /// <summary>
        /// AddPermission
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="enableType"></param>
        /// <returns></returns>
        public virtual PermissionDefinition AddPermission(
            string name,
            string displayName = null,
            string description = null,
            PermissionEnableType enableType = PermissionEnableType.Enable
            )
        {
            var permission = new PermissionDefinition(
                name,
                displayName,
                description,
                enableType
            )
            {
                PermissionGroup = this
            };

            _permissions.Add(permission);

            return permission;
        }

        /// <summary>
        /// GetPermissionsWithChildren
        /// </summary>
        /// <returns></returns>
        public virtual List<PermissionDefinition> GetPermissionsWithChildren()
        {
            var permissions = new List<PermissionDefinition>();

            foreach (var permission in _permissions)
            {
                AddPermissionToListRecursively(permissions, permission);
            }

            return permissions;
        }

        private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
        {
            permissions.Add(permission);

            foreach (var child in permission.Children)
            {
                AddPermissionToListRecursively(permissions, child);
            }
        }


        /// <summary>
        /// GetPermissionOrNull
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PermissionDefinition GetPermissionOrNull([NotNull] string name)
        {
            return GetPermissionOrNullRecursively(Permissions, name);
        }

        private PermissionDefinition GetPermissionOrNullRecursively(
            IReadOnlyList<PermissionDefinition> permissions, string name)
        {
            foreach (var permission in permissions)
            {
                if (permission.Name == name)
                {
                    return permission;
                }

                var childPermission = GetPermissionOrNullRecursively(permission.Children, name);
                if (childPermission != null)
                {
                    return childPermission;
                }
            }

            return null;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{nameof(PermissionGroupDefinition)} {Name}]";
        }
    }
}