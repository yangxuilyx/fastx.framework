namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// 权限提供者
    /// </summary>
    public interface IUserPermissionProvider
    {
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public Task<PermissionResult> GetPermissions(string uid, string tenantId);
    }
}