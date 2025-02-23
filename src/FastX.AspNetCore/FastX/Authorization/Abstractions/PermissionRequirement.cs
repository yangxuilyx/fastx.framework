using Microsoft.AspNetCore.Authorization;

namespace FastX.Authorization.Abstractions
{
    /// <summary>
    /// MutePermissionRequirement
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// PermissionName
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// MutePermissionRequirement
        /// </summary>
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}