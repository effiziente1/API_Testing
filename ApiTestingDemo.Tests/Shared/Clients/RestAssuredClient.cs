using static RestAssured.Dsl;

namespace ApiTestingDemo.Tests.Shared.Clients;

/// <summary>
/// RestAssured.Net client wrapper
/// Provides fluent API for HTTP requests with built-in schema validation support
/// </summary>
public class RestAssuredClient
{
    private readonly string _baseUrl;
    private readonly string? _authToken;

    /// <summary>
    /// Initializes a new instance of RestAssuredClient
    /// </summary>
    /// <param name="url">Base URL of the API</param>
    /// <param name="token">Optional JWT token for authentication</param>
    public RestAssuredClient(string url, string? token = null)
    {
        _baseUrl = url?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(url));
        _authToken = token;
    }

    /// <summary>
    /// Creates a configured request with authentication header
    /// This is the starting point for all RestAssured.Net requests
    /// </summary>
    /// <returns>ExecutableRequest with base configuration applied</returns>
    /// <example>
    /// var response = client.CreateRequest()
    ///     .When()
    ///     .Get(url)
    ///     .Then()
    ///     .StatusCode(200);
    /// </example>
    public global::RestAssured.Request.ExecutableRequest CreateRequest()
    {
        var request = Given();

        // Add JWT Bearer token if provided
        if (!string.IsNullOrEmpty(_authToken))
        {
            request = request.Header("Authorization", $"Bearer {_authToken}");
        }

        return request;
    }

    /// <summary>
    /// Combines base URL with endpoint to create full URL
    /// Handles proper slash trimming to avoid double slashes
    /// </summary>
    /// <param name="endpoint">The API endpoint (e.g., "/api/companies" or "api/companies")</param>
    /// <returns>Full URL (e.g., "https://api.example.com/api/companies")</returns>
    public string GetFullUrl(string endpoint)
    {
        return $"{_baseUrl}/{endpoint.TrimStart('/')}";
    }
}
