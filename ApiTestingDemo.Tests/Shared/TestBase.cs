using ApiTestingDemo.Tests.Shared.Models;
using ApiTestingDemo.Tests.Shared.Clients;
using Microsoft.Extensions.Configuration;

namespace ApiTestingDemo.Tests.Shared;

/// <summary>
/// Base class for all test classes
/// Handles common setup like configuration and authentication
/// </summary>
public abstract class TestBase
{
    /// <summary>
    /// JWT authentication token obtained during setup
    /// Available to all derived test classes
    /// </summary>
    protected string AuthToken { get; private set; } = string.Empty;

    /// <summary>
    /// Configuration instance for accessing environment variables and user secrets
    /// </summary>
    protected IConfiguration Configuration { get; private set; } = null!;

    /// <summary>
    /// Default timeout for API requests from test settings
    /// </summary>
    protected int DefaultTimeout { get; private set; }

    /// <summary>
    /// Number of retry attempts from test settings
    /// </summary>
    protected int RetryAttempts { get; private set; }

    /// <summary>
    /// Runs once before all tests in the class
    /// Configures settings and obtains authentication token
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        // Step 1: Build configuration from appsettings.json, environment variables and user secrets
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .AddUserSecrets<TestBase>()
            .Build();

        // Step 2: Read test settings from TestContext parameters (.runsettings)
        DefaultTimeout = int.Parse(TestContext.Parameters["DefaultTimeout"] ?? "30000");
        RetryAttempts = int.Parse(TestContext.Parameters["RetryAttempts"] ?? "3");

        // Step 3: Read user credentials from configuration (user secrets or environment variables)
        var company = Configuration["USER_COMPANY"]!;
        var userName = Configuration["USER_NAME"]!;
        var password = Configuration["USER_PASSWORD"]!;

        // Step 4: Create user object for authentication
        var user = new User
        {
            Company = company,
            UserName = userName,
            Password = password
        };

        // Step 5: Get API base URL from test context parameters
        var authUrl = TestContext.Parameters["AuthUrl"] ?? Configuration["AuthUrl"]!;

        // Step 6: Create API client (no auth token needed for login)
        var authClient = new RestSharpClient(authUrl);

        // Step 7: Authenticate and get JWT token
        var response = await authClient.PostAsync<LoginResponse>("/api/Users/login", user);
        AuthToken = response.Data!.Token;
    }
}
