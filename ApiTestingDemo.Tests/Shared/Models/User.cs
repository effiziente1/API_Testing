namespace ApiTestingDemo.Tests.Shared.Models;

/// <summary>
/// Represents user credentials for authentication
/// </summary>
public class User
{
    /// <summary>
    /// Company identifier for login
    /// </summary>
    public string Company { get; set; } = string.Empty;
    
    /// <summary>
    /// Username for authentication
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// User password (should be handled securely)
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
