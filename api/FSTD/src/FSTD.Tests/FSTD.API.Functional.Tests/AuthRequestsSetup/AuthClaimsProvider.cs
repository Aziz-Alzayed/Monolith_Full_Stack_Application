using FSTD.DataCore.Authentication;
using System.Security.Claims;

public class AuthClaimsProvider
{
    public IList<Claim> Claims { get; }

    public AuthClaimsProvider(IList<Claim> claims)
    {
        Claims = claims;
    }

    public AuthClaimsProvider()
    {
        Claims = new List<Claim>();
    }

    public static AuthClaimsProvider WithGuestClaims()
    {
        var provider = new AuthClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "user@example.com"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, AppRoles.User));

        return provider;
    }

    public static AuthClaimsProvider WithAdminClaims()
    {
        var provider = new AuthClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "admin@example.com"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, AppRoles.Admin));

        return provider;
    }
}
