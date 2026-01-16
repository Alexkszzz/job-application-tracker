# Testing Guide

This guide explains how to run unit tests for both the backend API and frontend web application.

## Backend Tests (.NET/C#)

The backend uses **xUnit** as the testing framework, along with:

- **Moq** - for mocking dependencies
- **FluentAssertions** - for readable assertions
- **Microsoft.AspNetCore.Mvc.Testing** - for integration testing

### Running Backend Tests

```bash
# Navigate to test project
cd apps/api/JobTracker.Api.Tests

# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Backend Test Structure

```
apps/api/JobTracker.Api.Tests/
├── Services/
│   └── AuthServiceTests.cs    # Tests for authentication service
└── JobTracker.Api.Tests.csproj
```

### Writing Backend Tests

Example test for AuthService:

```csharp
[Fact]
public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccessWithToken()
{
    // Arrange
    var request = new LoginRequest
    {
        Email = "test@example.com",
        Password = "Test@123"
    };

    // Mock UserManager to return a user
    _mockUserManager
        .Setup(x => x.FindByEmailAsync(request.Email))
        .ReturnsAsync(mockUser);

    // Act
    var result = await _authService.LoginAsync(request);

    // Assert
    result.Succeeded.Should().BeTrue();
    result.Response!.Token.Should().NotBeNullOrEmpty();
}
```

## Frontend Tests (React/TypeScript)

The frontend uses **Jest** as the testing framework, along with:

- **React Testing Library** - for testing React components
- **@testing-library/jest-dom** - for custom DOM matchers
- **@testing-library/user-event** - for simulating user interactions

### Running Frontend Tests

```bash
# Navigate to web project
cd apps/web

# Run all tests
npm test

# Run tests in watch mode (reruns on file changes)
npm run test:watch

# Run tests with coverage report
npm run test:coverage
```

### Frontend Test Structure

```
apps/web/src/
├── contexts/
│   └── __tests__/
│       └── AuthContext.test.tsx    # Tests for auth context
├── components/
│   └── __tests__/
│       └── (component tests here)
└── lib/
    └── api/
        └── __tests__/
            └── (API tests here)
```

### Writing Frontend Tests

Example test for AuthContext:

```typescript
it("should load user from localStorage with valid token", async () => {
  // Arrange - create mock JWT token
  const mockJwtPayload = {
    sub: "123",
    email: "test@example.com",
    exp: Math.floor(Date.now() / 1000) + 3600,
  };
  const mockToken = `header.${btoa(JSON.stringify(mockJwtPayload))}.signature`;
  localStorage.setItem("token", mockToken);

  // Act - render component
  render(
    <AuthProvider>
      <TestComponent />
    </AuthProvider>
  );

  // Assert - check authentication state
  await waitFor(() => {
    expect(screen.getByTestId("authenticated")).toHaveTextContent(
      "authenticated"
    );
  });
});
```

## Test Coverage

### Backend Coverage

Currently covered:

- ✅ AuthService registration (success and failure cases)
- ✅ AuthService login (valid credentials, invalid email, invalid password)

To add coverage for:

- ApplicationService CRUD operations
- Controller integration tests
- Validation tests

### Frontend Coverage

Currently covered:

- ✅ AuthContext initialization
- ✅ Token loading from localStorage
- ✅ Token expiration validation
- ✅ User authentication state

To add coverage for:

- Login/Register page forms
- ProtectedRoute component
- API client functions
- Application list and detail pages

## Best Practices

### Backend Testing

1. **Arrange-Act-Assert**: Structure tests in three clear sections
2. **Mock External Dependencies**: Use Moq to isolate the system under test
3. **Use FluentAssertions**: Write readable assertions like `result.Succeeded.Should().BeTrue()`
4. **Test Both Success and Failure**: Cover happy path and error cases
5. **Mock Configuration**: Provide test values for IConfiguration

### Frontend Testing

1. **Test User Behavior**: Focus on what users see and do, not implementation details
2. **Mock External APIs**: Use jest.mock() for API calls and routing
3. **Use waitFor**: Handle async operations properly
4. **Mock localStorage**: Provide clean test data for each test
5. **Test Accessibility**: Use semantic queries like `getByRole`, `getByLabelText`

## CI/CD Integration

To run tests in CI/CD pipeline:

```yaml
# GitHub Actions example
- name: Test Backend
  run: |
    cd apps/api/JobTracker.Api.Tests
    dotnet test --logger "trx;LogFileName=test-results.trx"

- name: Test Frontend
  run: |
    cd apps/web
    npm test -- --ci --coverage
```

## Troubleshooting

### Backend Issues

**Error: "Cannot find type or namespace"**

- Ensure project reference is added: `dotnet add reference ../JobTracker.Api/JobTracker.Api.csproj`

**Error: "NullReferenceException in tests"**

- Check that all required mocks are configured properly
- Verify IConfiguration mocking includes all required sections

### Frontend Issues

**Error: "Cannot find module @testing-library/dom"**

- Install missing dependency: `npm install --save-dev @testing-library/dom`

**Error: "Token expired" in tests**

- Ensure mock JWT tokens have `exp` claim set to future timestamp
- Use `Math.floor(Date.now() / 1000) + 3600` for 1 hour from now

**Error: "Unable to find element"**

- Use `waitFor()` for async operations
- Check that component is actually rendering the expected element
- Use `screen.debug()` to see rendered HTML

## Running All Tests

To run both backend and frontend tests:

```bash
# From repository root
cd apps/api/JobTracker.Api.Tests && dotnet test && cd ../../web && npm test
```

## Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Jest Documentation](https://jestjs.io/)
- [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/)
