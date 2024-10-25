
# Example of Integration Tests for Task Command Handler

This project contains integration tests for the Task Command Handler in the FSTD application. The tests verify that the task-adding functionality works correctly, ensuring proper integration between the application, database, and external services like user management.

## Project Structure

- **Test Setup**: The tests are set up using an **in-memory database**, with dependency injection configured to include repositories, AutoMapper, and the MediatR pipeline.
- **Mocking**: The tests use **Moq** to mock external dependencies like `IUserQueriesRepo`.
- **MediatR**: The tests make use of MediatR to handle commands and dispatch them to the appropriate handlers.

## Key Components

### 1. AddTaskCommandHandler
   - The handler responsible for adding new tasks to the database.
   - This test verifies that tasks are correctly added and persisted.

### 2. In-Memory Database
   - An **in-memory database** is used for testing purposes to isolate the tests and ensure no external dependencies.
   - The database is created before each test and deleted afterward, ensuring a clean state for each test run.

### 3. Mocked Repositories
   - **IUserQueriesRepo** is mocked using **Moq** to simulate user retrieval, ensuring the user-related logic works independently of the actual repository implementation.
   - Example:
     ```csharp
     _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
         .ReturnsAsync(user);
     ```

## Test Scenarios

### 1. Handle_AddsTaskSuccessfully_ReturnsTaskDto
   - This test validates that a task is successfully added when valid data and an existing user are provided.
   - Example:
     ```csharp
     var result = await mediator.Send(command);
     Assert.NotNull(result);
     Assert.Equal(addTaskDto.Name, result.Name);
     Assert.Equal(addTaskDto.Description, result.Description);
     ```

### 2. Handle_UserNotFound_ThrowsUnauthorizedAccessException
   - This test verifies that when the user is not found, an `UnauthorizedAccessException` is thrown.
   - Example:
     ```csharp
     await Assert.ThrowsAsync<UnauthorizedAccessException>(() => mediator.Send(command));
     ```

## How to Run the Tests

### Prerequisites

- .NET SDK installed (version 6.0 or higher).
- Moq library for mocking dependencies.
- An IDE like Visual Studio or Visual Studio Code.

### Running the Tests

1. Clone the repository and navigate to the project directory.
2. Install the required dependencies using `dotnet restore`.
3. Run the tests using the following command:

   ```bash
   dotnet test
   ```

   This will execute all the integration tests in the project and provide feedback on their pass/fail status.

## Important Notes

1. **In-Memory Database**:
   - The tests use an in-memory database (`UseInMemoryDatabase`), ensuring no external database dependencies. The database is created before each test and deleted afterward.
   - No connection strings are required as the database lives in memory.

2. **Moq for Mocking**:
   - The user repository (`IUserQueriesRepo`) is mocked using **Moq** to simulate various scenarios, like finding a user or throwing an exception when a user is not found.
   
3. **Test Cleanup**:
   - After each test, the database is deleted and the service provider is disposed of to ensure no side effects across test runs.
   - Example cleanup:
     ```csharp
     _dbContext.Database.EnsureDeleted();
     ((IDisposable)_serviceProvider).Dispose();
     ```

## Conclusion

These integration tests ensure that the `AddTaskCommandHandler` integrates well with the in-memory database, user repository, and MediatR pipeline. The use of an in-memory database and mocking makes the tests efficient, isolated, and easy to maintain.

---
## Author & Contact
**Author:** Aziz Alzayed
**Email:** aziz.alzayed@a-fitech.com
