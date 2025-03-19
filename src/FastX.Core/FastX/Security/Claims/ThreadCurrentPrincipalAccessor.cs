using FastX.DependencyInjection;
using System.Security.Claims;

namespace FastX.Security.Claims;

public class ThreadCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
{
    protected virtual ClaimsPrincipal GetClaimsPrincipal()
    {
        return (Thread.CurrentPrincipal as ClaimsPrincipal)!;
    }

    public ClaimsPrincipal Principal => GetClaimsPrincipal();
}
