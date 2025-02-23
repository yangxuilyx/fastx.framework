using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// PermissionDefinition
    /// </summary>
    public class PermissionDefinition
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
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        public PermissionDefinition Parent { get; private init; }

        /// <summary>
        /// PermissionGroup
        /// </summary>
        public PermissionGroupDefinition PermissionGroup { get; set; }

        /// <summary>
        /// Children
        /// </summary>
        public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();
        private readonly List<PermissionDefinition> _children;

        /// <summary>
        /// EnableType
        /// </summary>
        public PermissionEnableType EnableType { get; set; }

        /// <summary>
        /// PermissionDefinition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="enableType"></param>
        protected internal PermissionDefinition(
            [NotNull] string name,
            string displayName = null,
            string description = null,
            PermissionEnableType enableType = PermissionEnableType.Enable)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            EnableType = enableType;

            _children = new List<PermissionDefinition>();
        }

        /// <summary>
        /// AddChild
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="enableType"></param>
        /// <returns></returns>
        public virtual PermissionDefinition AddChild(
            [NotNull] string name,
            string displayName = null,
            string description = null,
            PermissionEnableType enableType = PermissionEnableType.Enable
        )
        {
            var child = new PermissionDefinition(
                name,
                displayName,
                description,
                enableType
                )
            {
                Parent = this,
                PermissionGroup = PermissionGroup
            };

            _children.Add(child);

            return child;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{nameof(PermissionDefinition)} {Name}]";
        }
    }
}