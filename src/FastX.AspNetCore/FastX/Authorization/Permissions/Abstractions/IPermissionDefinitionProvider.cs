namespace FastX.Authorization.Permissions.Abstractions
{
    /// <summary>
    /// IPermissionDefinitionProvider
    /// </summary>
    public interface IPermissionDefinitionProvider
    {
        /// <summary>
        /// Define
        /// </summary>
        /// <param name="context"></param>
        void Define(IPermissionDefinitionContext context);
    }
}