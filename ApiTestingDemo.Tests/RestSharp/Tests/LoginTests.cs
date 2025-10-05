using ApiTestingDemo.Tests.Shared;
using ApiTestingDemo.Tests.Shared.Clients;
using ApiTestingDemo.Tests.Shared.Models;
using Allure.NUnit.Attributes;

namespace ApiTestingDemo.Tests.RestSharp.Tests;

/// <summary>
/// Functional tests for Company API endpoints using RestSharp
/// These tests verify the actual business functionality of the API
/// </summary>
[AllureParentSuite("Login")]
[Category("Login")]
public class LoginTests : TestBase
{
    private IApiClient _apiClient = null!;
    private string _baseUrl = string.Empty;

    [SetUp]
    public void Setup()
    {
        _baseUrl = TestContext.Parameters["AuthUrl"]!;
        _apiClient = new RestSharpClient(_baseUrl, AuthToken);
    }

    /// <summary>
    /// Test: Login and obtain JWT token
    /// Demonstrates:
    /// - POST request with JSON body
    /// - Authentication flow
    /// - Extracting token from response
    /// </summary>
    [Test]
    [Category("Smoke")]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Step 1: Create API client without authentication (for login endpoint)
        // We don't need a token to call the login endpoint
        var client = new RestSharpClient(_baseUrl);

        // Step 2: Prepare login credentials
        // In real tests, these should come from configuration/user secrets
        var loginRequest = new User
        {
            Company = Configuration["USER_COMPANY"]!,
            UserName = Configuration["USER_NAME"]!,
            Password = Configuration["USER_PASSWORD"]!
        };

        // Step 3: Send POST request to login endpoint
        // The request body is automatically serialized to JSON
        var response = await client.PostAsync<LoginResponse>("/api/Users/login", loginRequest);

        // Step 4: Assert successful authentication
        Assert.That(response.IsSuccess, Is.True, "Login should be successful");
        Assert.That(response.StatusCode, Is.EqualTo(200), "Status code should be 200");

        // Step 5: Verify token is present in response
        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.That(response.Data!.Token, Is.Not.Empty, "Token should not be empty");

        // Step 6: Verify token format (JWT tokens have 3 parts separated by dots)
        var tokenParts = response.Data.Token.Split('.');
        Assert.That(tokenParts.Length, Is.EqualTo(3), "JWT token should have 3 parts");

        // Step 7: Verify token expiration is in the future
        Assert.That(response.Data.TokenExpiration, Is.GreaterThan(DateTime.UtcNow),
            "Token expiration should be in the future");
    }

    /// <summary>
    /// Test: Request without authentication fails
    /// Demonstrates:
    /// - What happens when calling protected endpoints without token
    /// - Error handling for authentication failures
    /// </summary>
    [Test]
    public async Task UnauthenticatedRequest_ToProtectedEndpoint_ReturnsUnauthorized()
    {
        // Step 1: Create API client WITHOUT authentication token
        var unauthenticatedClient = new RestSharpClient(_baseUrl);

        // Step 2: Try to access protected endpoint
        var response = await unauthenticatedClient.GetAsync<Company[]>("api/Server/1/Companies");

        // Step 3: Verify that request is rejected
        // The exact behavior depends on API implementation
        // Common responses: 401 Unauthorized or 403 Forbidden
        Assert.That(response.IsSuccess, Is.False, "Request without auth should fail");
        Assert.That(response.StatusCode, Is.EqualTo(401).Or.EqualTo(403),
            "Should return 401 Unauthorized or 403 Forbidden");

    }


}