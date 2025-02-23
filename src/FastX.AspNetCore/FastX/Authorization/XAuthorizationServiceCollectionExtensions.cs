using FastX.Authorization.Abstractions;
using FastX.Authorization.Permissions;
using FastX.Authorization.Permissions.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FastX.Authorization
{
    /// <summary>
    /// HisAuthorizationServiceCollectionExtensions
    /// </summary>
    public static class XAuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// AddHisAuthorization
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXAuthorization(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IAuthorizationHandler, PermissionRequirementHandler>());
            services.TryAddTransient<IAuthorizationPolicyProvider, XAuthorizationPolicyProvider>();

            services.TryAddTransient<IXAuthorizationPolicyProvider, XAuthorizationPolicyProvider>();
            services.TryAddTransient<IAuthorizationService, XAuthorizationService>();
            services.TryAddTransient<IUserPermissionProvider, NullUserPermissionProvider>();
            services.TryAddTransient<IPermissionChecker, PermissionChecker>();

            services.TryAddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();

            services.AddAuthorizationCore();

            return services;
        }
    }
}