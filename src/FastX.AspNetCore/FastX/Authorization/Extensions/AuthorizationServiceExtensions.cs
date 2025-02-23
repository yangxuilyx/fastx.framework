using FastX.Authorization.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace FastX.Authorization.Extensions
{
    /// <summary>
    /// AuthorizationServiceExtensions
    /// </summary>
    public static class AuthorizationServiceExtensions
    {
        /// <summary>
        /// AuthorizeAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName)
        {
            return await AuthorizeAsync(
                authorizationService,
                null,
                policyName
            );
        }

        /// <summary>
        /// AuthorizeAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsHisAuthorizationService().CurrentPrincipal,
                resource,
                requirement
            );
        }

        /// <summary>
        /// AuthorizeAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsHisAuthorizationService().CurrentPrincipal,
                resource,
                policy
            );
        }

        /// <summary>
        /// AuthorizeAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            return await AuthorizeAsync(
                authorizationService,
                null,
                policy
            );
        }

        /// <summary>
        /// AuthorizeAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsHisAuthorizationService().CurrentPrincipal,
                resource,
                requirements
            );
        }

        /// <summary>
        /// AuthorizeAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsHisAuthorizationService().CurrentPrincipal,
                resource,
                policyName
            );
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName)
        {
            var isGrantedAsync = (await authorizationService.AuthorizeAsync(policyName)).Succeeded;
            return isGrantedAsync;
        }

        /// <summary>
        /// IsGrantedAnyAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policyNames"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAnyAsync(
            this IAuthorizationService authorizationService,
            params string[] policyNames)
        {
            foreach (var policyName in policyNames)
            {
                if ((await authorizationService.AuthorizeAsync(policyName)).Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirement)).Succeeded;
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(resource, policy)).Succeeded;
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(policy)).Succeeded;
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirements)).Succeeded;
        }

        /// <summary>
        /// IsGrantedAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(resource, policyName)).Succeeded;
        }

        /// <summary>
        /// CheckAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(policyName))
            {
                throw new Exception($"{policyName}未授权");
            }
        }

        /// <summary>
        /// CheckAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirement))
            {
                throw new Exception("未授权");
            }
        }

        /// <summary>
        /// CheckAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policy))
            {
                throw new Exception("未授权");
            }
        }

        /// <summary>
        /// CheckAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static async Task CheckAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(policy))
            {
                throw new Exception("未授权");
            }
        }

        /// <summary>
        /// CheckAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirements))
            {
                throw new Exception("未授权");
            }
        }

        /// <summary>
        /// CheckAsync
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policyName))
            {
                throw new Exception("未授权");
            }
        }

        private static IXAuthorizationService AsHisAuthorizationService(this IAuthorizationService authorizationService)
        {
            if (!(authorizationService is IXAuthorizationService hisAuthorizationService))
            {
                throw new Exception($"{nameof(authorizationService)} should implement {typeof(IXAuthorizationService).FullName}");
            }

            return hisAuthorizationService;
        }
    }
}
