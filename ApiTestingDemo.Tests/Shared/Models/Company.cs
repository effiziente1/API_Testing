namespace ApiTestingDemo.Tests.Shared.Models;

/// <summary>
/// Represents a company entity from the Microsip API
/// </summary>
public class Company
{
    /// <summary>
    /// Unique identifier for the company
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Server ID where the company data is stored
    /// </summary>
    public int ServerId { get; set; }
    
    /// <summary>
    /// Company key identifier
    /// </summary>
    public int Key { get; set; }
    
    /// <summary>
    /// Company name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Database name associated with the company
    /// </summary>
    public string DataBase { get; set; } = string.Empty;
    
    /// <summary>
    /// Indicates whether the company is active
    /// </summary>
    public bool Active { get; set; }
}
