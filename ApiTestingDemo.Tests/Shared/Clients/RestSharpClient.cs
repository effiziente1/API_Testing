using ApiTestingDemo.Tests.Shared.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;

namespace ApiTestingDemo.Tests.Shared.Clients;

/// <summary>
/// RestSharp implementation of IApiClient
/// Provides HTTP client functionality using RestSharp library with JWT authentication support
/// </summary>
public class RestSharpClient : IApiClient
{
    private readonly RestClient _client;

    /// <summary>
    /// Initializes a new instance of RestSharpClient
    /// </summary>
    /// <param name="url">Base URL of the API</param>
    /// <param name="token">Optional JWT token for authentication</param>
    public RestSharpClient(string url, string? token = null)
    {
        var options = new RestClientOptions(url);

        // Configure JWT authentication if token is provided
        // RestSharp's JwtAuthenticator automatically adds the Bearer token to requests
        if (!string.IsNullOrEmpty(token))
        {
            options.Authenticator = new JwtAuthenticator(token);
        }

        // Configure timeout from test context if available
        var timeOut = TestContext.Parameters["TimeOut"];
        if (int.TryParse(timeOut, out var timeoutValue))
        {
            options.Timeout = TimeSpan.FromMilliseconds(timeoutValue);
        }

        _client = new RestClient(options);
    }

    /// <summary>
    /// Sends a GET request to the specified endpoint
    /// </summary>
    public async Task<ApiResponse> GetAsync(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Get);
        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response);
        return ProcessApiResponse(response);
    }

    /// <summary>
    /// Sends a GET request and deserializes the response to the specified type
    /// </summary>
    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Get);
        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response);
        return ProcessApiResponse<T>(response);
    }

    /// <summary>
    /// Sends a POST request with JSON body
    /// </summary>
    public async Task<ApiResponse> PostAsync(string endpoint, object body)
    {
        var request = new RestRequest(endpoint, Method.Post);
        request.AddJsonBody(body);

        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response, body);
        return ProcessApiResponse(response);
    }

    /// <summary>
    /// Sends a POST request with JSON body and deserializes the response
    /// </summary>
    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object body)
    {
        var request = new RestRequest(endpoint, Method.Post);
        request.AddJsonBody(body);

        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response, body);
        return ProcessApiResponse<T>(response);
    }

    /// <summary>
    /// Sends a PUT request with JSON body
    /// </summary>
    public async Task<ApiResponse> PutAsync(string endpoint, object body)
    {
        var request = new RestRequest(endpoint, Method.Put);
        request.AddJsonBody(body);

        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response, body);
        return ProcessApiResponse(response);
    }

    /// <summary>
    /// Sends a PUT request with JSON body and deserializes the response
    /// </summary>
    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object body)
    {
        var request = new RestRequest(endpoint, Method.Put);
        request.AddJsonBody(body);

        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response, body);
        return ProcessApiResponse<T>(response);
    }

    /// <summary>
    /// Sends a DELETE request to the specified endpoint
    /// </summary>
    public async Task<ApiResponse> DeleteAsync(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Delete);
        var response = await _client.ExecuteAsync(request);
        LogRequestResponse(request, response);
        return ProcessApiResponse(response);
    }

    /// <summary>
    /// Processes RestSharp response into ApiResponse model
    /// </summary>
    private ApiResponse ProcessApiResponse(RestResponse response)
    {
        LogServerErrorIfNeeded(response);

        return new ApiResponse
        {
            StatusCode = (int)response.StatusCode,
            Content = response.Content ?? string.Empty,
            IsSuccess = response.IsSuccessful,
            ErrorMessage = response.ErrorMessage ?? response.ErrorException?.Message
        };
    }

    /// <summary>
    /// Processes RestSharp response into typed ApiResponse model
    /// Automatically deserializes JSON content to the specified type
    /// </summary>
    private ApiResponse<T> ProcessApiResponse<T>(RestResponse response)
    {
        LogServerErrorIfNeeded(response);

        var apiResponse = MapResponseBase<T>(response);

        // Deserialize response content if request was successful
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            apiResponse.Data = JsonSerializer.Deserialize<T>(response.Content);
        }

        return apiResponse;
    }

    /// <summary>
    /// Maps common response properties from RestResponse to ApiResponse
    /// </summary>
    private ApiResponse<T> MapResponseBase<T>(RestResponse response)
    {
        return new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            Content = response.Content!,
            IsSuccess = response.IsSuccessful,
            ErrorMessage = response.ErrorMessage
        };
    }

    /// <summary>
    /// Logs server errors (5xx status codes) for debugging purposes
    /// </summary>
    private void LogServerErrorIfNeeded(RestResponse response)
    {
        if ((int)response.StatusCode >= 500)
        {
            Console.WriteLine("Server error: " + response.Content);
            Console.WriteLine("Server url: " + response.ResponseUri);

        }
    }

    /// <summary>
    /// Logs request and response details when status code >= 400
    /// </summary>
    private void LogRequestResponse(RestRequest request, RestResponse response, object? body = null)
    {
        if ((int)response.StatusCode >= 400)
        {
            Console.WriteLine("========== HTTP ERROR ==========");
            Console.WriteLine($"Method: {request.Method}");
            Console.WriteLine($"URL: {response.ResponseUri}");
            Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");
            
            if (body != null)
            {
                Console.WriteLine($"Request Body: {JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true })}");
            }
            
            Console.WriteLine($"Response Body: {response.Content}");
            Console.WriteLine("================================");
        }
    }
}
