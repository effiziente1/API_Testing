using ApiTestingDemo.Tests.Shared.Models;
using ApiTestingDemo.Tests.Shared.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    
    private ServiceProvider _serviceProvider = null!;

    /// <summary>
    /// Runs once before all tests in the class
    /// Configures DI container and obtains authentication token
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        // Step 1: Create dependency injection container
        _serviceProvider = CreateServiceProvider();

        // Step 2: Get configuration from DI container
        Configuration = _serviceProvider.GetRequiredService<IConfiguration>();

        // Step 3: Read test settings from TestContext parameters (.runsettings)
        DefaultTimeout = int.Parse(TestContext.Parameters["DefaultTimeout"] ?? "30000");
        RetryAttempts = int.Parse(TestContext.Parameters["RetryAttempts"] ?? "3");

        // Step 4: Read user credentials from configuration (user secrets or environment variables)
        var company = Configuration["USER_COMPANY"]!;
        var userName = Configuration["USER_NAME"]!;
        var password = Configuration["USER_PASSWORD"]!;

        // Step 5: Create user object for authentication
        var user = new User
        {
            Company = company,
            UserName = userName,
            Password = password
        };
        
        // Step 6: Get API base URL from test context parameters
        var authUrl = TestContext.Parameters["AuthUrl"]!;
        
        // Step 7: Create API client (no auth token needed for login)
        var authClient = new RestSharpClient(authUrl);
        
        // Step 8: Authenticate and get JWT token
        var response = await authClient.PostAsync<LoginResponse>("/api/Users/login", user);
        AuthToken = response.Data!.Token;
    }

    /// <summary>
    /// Runs once after all tests in the class
    /// Cleans up resources
    /// </summary>
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _serviceProvider?.Dispose();
    }

    /// <summary>
    /// Creates and configures the service provider
    /// </summary>
    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Configures dependency injection services
    /// Sets up configuration from multiple sources (environment variables, user secrets)
    /// </summary>
    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<TestBase>()
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
    }
}
