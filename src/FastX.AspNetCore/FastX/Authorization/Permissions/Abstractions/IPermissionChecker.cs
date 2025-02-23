using System.Security.Claims;

namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// IPermissionChecker
    /// </summary>
    public interface IPermissionChecker
    {
        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> IsGrantedAsync(string name);

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name);
    }
}