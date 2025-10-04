namespace ApiTestingDemo.Tests.Shared.Models;

/// <summary>
/// Generic API response wrapper containing status information and content
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// HTTP status code of the response
    /// </summary>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// Raw response content as string
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Error message if the request failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Indicates whether the request was successful
    /// </summary>
    public bool IsSuccess { get; set; } = true;
    
    /// <summary>
    /// Time taken for the request to complete
    /// </summary>
    public TimeSpan ResponseTime { get; set; }
}

/// <summary>
/// Generic API response wrapper with typed data
/// </summary>
/// <typeparam name="T">Type of the response data</typeparam>
public class ApiResponse<T> : ApiResponse
{
    /// <summary>
    /// Deserialized response data
    /// </summary>
    public T? Data { get; set; }
}

