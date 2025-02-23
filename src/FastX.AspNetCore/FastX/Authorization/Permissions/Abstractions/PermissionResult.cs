namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// 权限列表
    /// </summary>
    public class PermissionResult
    {
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<string> Permissions { get; set; } = new();

        /// <summary>
        /// 是否超管
        /// </summary>
        public bool Special { get; set; }
    }
}