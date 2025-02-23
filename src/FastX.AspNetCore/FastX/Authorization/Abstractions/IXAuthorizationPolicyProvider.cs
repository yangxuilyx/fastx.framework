using Microsoft.AspNetCore.Authorization;

namespace FastX.Authorization.Abstractions
{
    /// <summary>
    /// IHisAuthorizationPolicyProvider
    /// </summary>
    public interface IXAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        /// <summary>
        /// GetPoliciesNamesAsync
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetPoliciesNamesAsync();
    }
}