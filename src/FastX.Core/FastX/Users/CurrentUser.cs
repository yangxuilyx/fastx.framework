using System.Security.Claims;
using FastX.DependencyInjection;
using FastX.Security.Claims;

namespace FastX.Users;

public class CurrentUser : ICurrentUser, ITransientDependency
{
    private readonly ICurrentPrincipalAccessor _principalAccessor;

    public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }

    public string? UserId => FindClaim(XClaimTypes.UserId)?.Value;

    public string? UserName => FindClaim(XClaimTypes.UserName)?.Value;

    public string? Name => FindClaim(XClaimTypes.Name)?.Value;

    public virtual Claim? FindClaim(string claimType)
    {
        return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }
}