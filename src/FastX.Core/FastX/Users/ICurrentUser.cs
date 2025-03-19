using System.Security.Claims;

namespace FastX.Users;

public interface ICurrentUser
{
    string? UserId { get; }

    string? UserName { get; }

    public string? Name { get; }

    Claim? FindClaim(string claimType);
}