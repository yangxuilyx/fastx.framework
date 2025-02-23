using FastX.Authorization.Abstractions;
using FastX.Authorization.Extensions;
using FastX.Authorization.Permissions.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FastX.Authorization
{
    /// <summary>
    /// HisAuthorizationPolicyProvider
    /// </summary>
    public class XAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, IXAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        /// <summary>
        /// Creates a new instance of <see cref="T:Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider" />.
        /// </summary>
        /// <param name="options">The options used to configure this instance.</param>
        /// <param name="permissionDefinitionManager"></param>
        public XAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionDefinitionManager permissionDefinitionManager) : base(options)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _options = options.Value;
        }

        /// <summary>
        /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.</returns>
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            var builder = new AuthorizationPolicyBuilder(Array.Empty<string>());
            builder.Requirements.Add(new PermissionRequirement(policyName));

            return builder.Build();
        }

        /// <summary>
        /// GetPoliciesNamesAsync
        /// </summary>
        /// <returns></returns>
        public Task<List<string>> GetPoliciesNamesAsync()
        {
            return Task.FromResult(
                _options.GetPoliciesNames()
                    .Union(
                        _permissionDefinitionManager
                            .GetPermissions()
                            .Select(p => p.Name)
                    )
                    .ToList()
            );
        }
    }
}