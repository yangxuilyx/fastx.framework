using System.Security.Claims;
using FastX.Authorization.Permissions.Abstractions;
using FastX.Security.Claims;
using FastX.Users;

namespace FastX.Authorization.Permissions
{
    /// <summary>
    /// PermissionChecker
    /// </summary>
    public class PermissionChecker : IPermissionChecker
    {
        /// <summary>
        /// PermissionDefinitionManager
        /// </summary>
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        /// <summary>
        /// CurrentPrincipalAccessor
        /// </summary>
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }

        /// <summary>
        /// UserPermissionProvider
        /// </summary>
        protected IUserPermissionProvider UserPermissionProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        public PermissionChecker(IPermissionDefinitionManager permissionDefinitionManager,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IUserPermissionProvider userPermissionProvider)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            CurrentPrincipalAccessor = currentPrincipalAccessor;
            UserPermissionProvider = userPermissionProvider;
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> IsGrantedAsync(string name)
        {
            return await IsGrantedAsync(CurrentPrincipalAccessor.Principal, name);
        }

        private async Task<PermissionResult> GetCheckResult(ClaimsPrincipal claimsPrincipal)
        {
            return await UserPermissionProvider.GetPermissions(
                claimsPrincipal.Claims.FirstOrDefault(p => p.Type == XClaimTypes.UserId)?.Value,
                claimsPrincipal.Claims.FirstOrDefault(p => p.Type == XClaimTypes.TenantId)?.Value);
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            var permissionResult = await GetCheckResult(claimsPrincipal);
            if (permissionResult.Special)
                return true;

            return permissionResult.Permissions.Any(p => p == name);
        }
    }
}