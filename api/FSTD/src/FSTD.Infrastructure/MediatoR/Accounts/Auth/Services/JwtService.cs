using FSTD.Application.MediatoR.Accounts.Auth.Services;
using FSTD.DataCore.Models.JwtModels;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FSTD.Infrastructure.MediatoR.Accounts.Auth.Services
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class JwtService : IJwtService
    {
        public IImmutableDictionary<string, JwtRefreshModel> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
        private readonly ConcurrentDictionary<string, JwtRefreshModel> _usersRefreshTokens; // can store in a database or a distributed cache
        private readonly JwtTokenConfigModel _jwtTokenConfig;
        private readonly byte[] _secretKey;

        public JwtService(JwtTokenConfigModel jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _usersRefreshTokens = new ConcurrentDictionary<string, JwtRefreshModel>();
            _secretKey = Encoding.UTF8.GetBytes(jwtTokenConfig.SecretKey);
        }

        // optional: clean up expired refresh tokens
        public void RemoveExpiredRefreshTokens(DateTime now)
        {
            try
            {
                var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();

                foreach (var expiredToken in expiredTokens)
                {
                    _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // can be more specific to ip, user agent, device name, etc.
        public void RemoveRefreshTokenByUserName(string userEmail)
        {
            try
            {
                var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserEmail == userEmail).ToList();

                foreach (var refreshToken in refreshTokens)
                {
                    _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Generate Acess token with refresh token.
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="claims">Roles</param>
        /// <param name="now">Start date of the access token.</param>
        /// <returns></returns>
        public JwtAuthModel GenerateTokens(string userEmail, Claim[] claims, DateTime now)
        {
            try
            {
                var key = new SymmetricSecurityKey(_secretKey);
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
                var jwtToken = new JwtSecurityToken
                    (
                    issuer: _jwtTokenConfig.Issuer,
                    audience: shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                    claims: claims,
                    expires: now.AddDays(_jwtTokenConfig.AccessTokenExpirationDays),
                    signingCredentials: credentials
                    );
                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                var refreshToken = new JwtRefreshModel
                {
                    UserEmail = userEmail,
                    RefreshToken = GenerateRefreshTokenString(),
                    ExpireAt = now.AddDays(_jwtTokenConfig.RefreshTokenExpirationDays)
                };
                _usersRefreshTokens.AddOrUpdate(refreshToken.RefreshToken, refreshToken, (s, t) => refreshToken);

                return new JwtAuthModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.RefreshToken
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JwtAuthModel Refresh(string refreshToken, string accessToken, DateTime now)
        {
            try
            {
                var (principal, jwtToken) = DecodeJwtToken(accessToken);
                // Decode the token
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var userEmail = principal.Identity.Name;

                if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
                {
                    throw new SecurityTokenException("Invalid token");
                }
                if (existingRefreshToken.UserEmail != userEmail || DateTime.Compare(now, existingRefreshToken.ExpireAt) > 0)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return GenerateTokens(userEmail, principal.Claims.ToArray(), now); // need to recover the original claims
            }
            catch
            {

                throw;
            }
        }

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                    ValidAudience = _jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                }, out var validatedToken);

                return (principal, validatedToken as JwtSecurityToken);
            }
            catch
            {

                throw;
            }
        }
        public static string GenerateRefreshTokenString()
        {
            try
            {
                var randomNumber = new byte[32];
                using var randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
            catch
            {

                throw;
            }
        }
    }
}
