using ApiTestingDemo.Tests.Shared.Models;

namespace ApiTestingDemo.Tests.Shared.Clients;

/// <summary>
/// Generic interface for API client implementations
/// Provides common HTTP operations (GET, POST, PUT, DELETE)
/// </summary>
public interface IApiClient
{
    /// <summary>
    /// Performs a GET request to the specified endpoint
    /// </summary>
    /// <param name="endpoint">API endpoint path</param>
    /// <returns>API response without typed data</returns>
    Task<ApiResponse> GetAsync(string endpoint);
    
    /// <summary>
    /// Performs a GET request and deserializes the response to type T
    /// </summary>
    /// <typeparam name="T">Expected response type</typeparam>
    /// <param name="endpoint">API endpoint path</param>
    /// <returns>API response with typed data</returns>
    Task<ApiResponse<T>> GetAsync<T>(string endpoint);
    
    /// <summary>
    /// Performs a POST request with a JSON body
    /// </summary>
    /// <param name="endpoint">API endpoint path</param>
    /// <param name="body">Request body object</param>
    /// <returns>API response without typed data</returns>
    Task<ApiResponse> PostAsync(string endpoint, object body);
    
    /// <summary>
    /// Performs a POST request and deserializes the response to type T
    /// </summary>
    /// <typeparam name="T">Expected response type</typeparam>
    /// <param name="endpoint">API endpoint path</param>
    /// <param name="body">Request body object</param>
    /// <returns>API response with typed data</returns>
    Task<ApiResponse<T>> PostAsync<T>(string endpoint, object body);
    
    /// <summary>
    /// Performs a PUT request with a JSON body
    /// </summary>
    /// <param name="endpoint">API endpoint path</param>
    /// <param name="body">Request body object</param>
    /// <returns>API response without typed data</returns>
    Task<ApiResponse> PutAsync(string endpoint, object body);
    
    /// <summary>
    /// Performs a PUT request and deserializes the response to type T
    /// </summary>
    /// <typeparam name="T">Expected response type</typeparam>
    /// <param name="endpoint">API endpoint path</param>
    /// <param name="body">Request body object</param>
    /// <returns>API response with typed data</returns>
    Task<ApiResponse<T>> PutAsync<T>(string endpoint, object body);
    
    /// <summary>
    /// Performs a DELETE request to the specified endpoint
    /// </summary>
    /// <param name="endpoint">API endpoint path</param>
    /// <returns>API response without typed data</returns>
    Task<ApiResponse> DeleteAsync(string endpoint);
}
