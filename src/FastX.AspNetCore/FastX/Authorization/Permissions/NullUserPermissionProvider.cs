using FastX.Authorization.Permissions.Abstractions;

namespace FastX.Authorization.Permissions
{
    /// <summary>
    /// 权限提供
    /// </summary>
    public class NullUserPermissionProvider : IUserPermissionProvider
    {
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionResult> GetPermissions(string uid, string eid)
        {
            return await Task.FromResult(new PermissionResult() { });
        }
    }
}