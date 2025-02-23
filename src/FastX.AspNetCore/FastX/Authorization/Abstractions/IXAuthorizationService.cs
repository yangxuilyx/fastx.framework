using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FastX.Authorization.Abstractions
{
    /// <summary>
    /// IHisAuthorizationService
    /// </summary>
    public interface IXAuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// CurrentPrincipal
        /// </summary>
        ClaimsPrincipal CurrentPrincipal { get; }
    }
}