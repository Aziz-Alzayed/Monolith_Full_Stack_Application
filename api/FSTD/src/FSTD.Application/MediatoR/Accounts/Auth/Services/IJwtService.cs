using FSTD.DataCore.Models.JwtModels;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FSTD.Application.MediatoR.Accounts.Auth.Services
{
    public interface IJwtService
    {
        IImmutableDictionary<string, JwtRefreshModel> UsersRefreshTokensReadOnlyDictionary { get; }
        JwtAuthModel GenerateTokens(string userEmail, Claim[] claims, DateTime now);
        JwtAuthModel Refresh(string refreshToken, string accessToken, DateTime now);
        void RemoveExpiredRefreshTokens(DateTime now);
        void RemoveRefreshTokenByUserName(string userEmail);
        (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
    }
}
