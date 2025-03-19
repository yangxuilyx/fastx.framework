using System.Security.Claims;

namespace FastX.Security.Claims;

public interface ICurrentPrincipalAccessor
{
    ClaimsPrincipal Principal { get; }
}
