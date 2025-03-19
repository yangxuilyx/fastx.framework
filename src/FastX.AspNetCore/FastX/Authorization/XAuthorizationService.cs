using System.Security.Claims;
using FastX.Authorization.Abstractions;
using FastX.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FastX.Authorization
{
    /// <summary>
    /// HisAuthorizationService
    /// </summary>
    public class XAuthorizationService : DefaultAuthorizationService, IXAuthorizationService
    {
        /// <summary>
        /// CurrentPrincipal
        /// </summary>
        public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyProvider"></param>
        /// <param name="handlers"></param>
        /// <param name="logger"></param>
        /// <param name="contextFactory"></param>
        /// <param name="evaluator"></param>
        /// <param name="options"></param>
        /// <param name="currentPrincipalAccessor"></param>
        public XAuthorizationService(IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            ILogger<DefaultAuthorizationService> logger,
            IAuthorizationHandlerContextFactory contextFactory,
            IAuthorizationEvaluator evaluator,
            IOptions<AuthorizationOptions> options, ICurrentPrincipalAccessor currentPrincipalAccessor) : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
        }
    }
}