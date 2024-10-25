
# Example of Functional Tests for Admin API

This project contains functional tests for the Admin API, validating authentication, authorization, and role-based access control. The tests ensure that various user roles, including admin and regular users, can access the appropriate resources while unauthorized access is restricted.

## Project Structure

- **Test Setup**: The project uses a custom `CustomWebApiFactory` that configures an in-memory database and injects authentication claims to simulate different user roles.
- **Authentication**: Custom `TestAuthHandler` is used to override the default authentication mechanism, allowing for dynamic user claims during tests.
- **Database**: The tests rely on an **in-memory database** for fast and isolated test executions, seeded with users of different roles (`SuperUser`, `Admin`, `User`).

## Key Components

### 1. AuthClaimsProvider
   - A helper class to create custom authentication claims for users of different roles (Admin, SuperUser, Guest).
   - Example:
     ```csharp
     public static AuthClaimsProvider WithAdminClaims() { /* returns admin claims */ }
     public static AuthClaimsProvider WithGuestClaims() { /* returns guest claims */ }
     ```

### 2. CustomWebApiFactory
   - A factory that configures the test environment, setting up an in-memory database, authentication schemes, and database seeding.
   - The factory is responsible for ensuring that each test runs in an isolated environment with pre-seeded data.
   - Example:
     ```csharp
     builder.ConfigureTestServices(services => { 
         services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
     });
     ```

### 3. TestAuthHandler
   - A custom authentication handler that simulates different users by injecting claims dynamically into the test requests.
   - It allows you to test the API's behavior under various authentication and authorization scenarios.

### 4. Test Scenarios
   - **GetAllUser_ShouldReturnOk_WhenUserIsAdmin**: Tests that an Admin can successfully retrieve the list of all users.
   - **GetAllUser_ShouldReturnForbidden_WhenUserIsNotAdminOrSuper**: Ensures that users without Admin or SuperUser roles are forbidden from accessing restricted endpoints.

## How to Run the Tests

### Prerequisites

- .NET SDK installed (preferably version 6.0 or higher).
- An IDE or text editor like Visual Studio or Visual Studio Code.

### Running the Tests

1. Clone the repository and navigate to the project directory.
2. Install any required dependencies using `dotnet restore`.
3. Run the tests with the following command:

   ```bash
   dotnet test
   ```

   This will execute all the functional tests in the project and provide feedback on whether they pass or fail.

## Important Notes

1. **In-Memory Database**: 
   - The tests use an **in-memory database** (`UseInMemoryDatabase("TestDb")`), ensuring each test run is isolated and has no side effects on the actual database.
   - No connection strings are required since the tests do not interact with a real database.

2. **Seeding Data**:
   - The database is automatically seeded with roles (`SuperUser`, `Admin`, `User`) and users for testing.
   - Seeding happens in the `CustomWebApiFactory` after the services are configured.

3. **Custom Authentication**:
   - The `TestAuthHandler` is used to inject dynamic user claims into the test requests, simulating different roles.
   - This allows you to test how the API responds to different user roles without the need for real tokens or OAuth.

4. **Dynamic Claims**:
   - The `AuthClaimsProvider` class is used to define the claims of different users (such as `Admin` or `Guest`) by modifying their roles dynamically.

## Example Test Case

```csharp
[Fact]
public async Task GetAllUser_ShouldReturnOk_WhenUserIsAdmin()
{
    var client = await CreateClientWithAuthorizationAsync(AuthClaimsProvider.WithAdminClaims());

    var response = await client.GetAsync("/api/admin/GetAllUser");
    response.EnsureSuccessStatusCode();

    var jsonString = await response.Content.ReadAsStringAsync();
    var users = JsonSerializer.Deserialize<List<UserFullInfoDto>>(jsonString, _jsonOptions);

    Assert.NotNull(users);
    Assert.Equal(3, users.Count);
    VerifyUser(users.FirstOrDefault(u => u.Email == "superuser@example.com"), "Super", "Super", "Super");
    VerifyUser(users.FirstOrDefault(u => u.Email == "admin@example.com"), "Admin", "Admin", "Admin");
    VerifyUser(users.FirstOrDefault(u => u.Email == "user@example.com"), "User", "User", "User");
}
```

## Conclusion

These functional tests ensure that the Admin API's authorization system works as expected, restricting access based on user roles and providing appropriate responses to authorized users. The use of in-memory databases and custom authentication handlers makes the tests efficient, fast, and reliable.

---
## Author & Contact
**Author:** Aziz Alzayed
**Email:** aziz.alzayed@a-fitech.com