using System.Security.Claims;

namespace FastX.Users;

public interface ICurrentPrincipalAccessor
{
    ClaimsPrincipal Principal { get; }
}
