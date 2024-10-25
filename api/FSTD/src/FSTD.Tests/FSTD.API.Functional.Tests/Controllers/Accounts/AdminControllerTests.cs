using FSTD.API.Functional.Tests.AuthRequestsSetup;
using FSTD.Application.DTOs.Accounts.Admins;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FSTD.API.Functional.Tests.Controllers.Accounts
{
    [ExcludeFromCodeCoverage]
    public class AdminControllerTests : IClassFixture<CustomWebApiFactory>
    {
        private readonly CustomWebApiFactory _factory;
        private readonly JsonSerializerOptions _jsonOptions;

        public AdminControllerTests(CustomWebApiFactory factory)
        {
            _factory = factory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private HttpClient CreateClientWithAuthorization(AuthClaimsProvider claimsProvider)
        {
            var client = _factory.CreateClientWithClaim(claimsProvider);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            return client;
        }

        private void VerifyUser(UserFullInfoDto user, string expectedFirstName, string expectedLastName, string expectedRole)
        {
            Assert.NotNull(user);
            Assert.Equal(expectedFirstName, user.FirstName);
            Assert.Equal(expectedLastName, user.LastName);
            Assert.Contains(expectedRole, user.Roles);
        }

        [Fact]
        public async Task GetAllUser_ShouldReturnOk_WhenUserIsAdmin()
        {
            // Arrange
            var client = CreateClientWithAuthorization(AuthClaimsProvider.WithAdminClaims());

            // Act
            var response = await client.GetAsync("/api/admin/GetAllUser");
            response.EnsureSuccessStatusCode();

            // Deserialize the response content
            var jsonString = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserFullInfoDto>>(jsonString, _jsonOptions);

            // Assert the user list contains 3 users
            Assert.NotNull(users);
            Assert.Equal(3, users.Count);

            // Verify each user's data
            var superUser = users.FirstOrDefault(u => u.Email == "superuser@example.com");
            Assert.NotNull(superUser);
            VerifyUser(superUser, "Super", "Super", "Super");

            var adminUser = users.FirstOrDefault(u => u.Email == "admin@example.com");
            Assert.NotNull(adminUser);
            VerifyUser(adminUser, "Admin", "Admin", "Admin");

            var normalUser = users.FirstOrDefault(u => u.Email == "user@example.com");
            Assert.NotNull(normalUser);
            VerifyUser(normalUser, "User", "User", "User");

        }

        [Fact]
        public async Task GetAllUser_ShouldReturnForbidden_WhenUserIsNotAdminOrSuper()
        {
            // Arrange
            var client = CreateClientWithAuthorization(AuthClaimsProvider.WithGuestClaims());

            // Act
            var response = await client.GetAsync("/api/admin/GetAllUser");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
