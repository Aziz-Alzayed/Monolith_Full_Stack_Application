using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FSTD.API.Functional.Tests.AuthRequestsSetup
{
    public class TestAuthHandler(
     IOptionsMonitor<AuthenticationSchemeOptions> options,
     ILoggerFactory logger,
     UrlEncoder encoder,
     AuthClaimsProvider claimsProvider) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        public const string SchemeName = "Test";
        private readonly IList<Claim> _claims = claimsProvider.Claims;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check if claims are properly populated
            if (_claims == null || !_claims.Any())
            {
                return Task.FromResult(AuthenticateResult.Fail("No claims provided"));
            }

            // Log or inspect claims
            foreach (var claim in _claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var identity = new ClaimsIdentity(_claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

    }
}
