namespace ApiTestingDemo.Tests.Shared.Helpers;

/// <summary>
/// Constants for JSON schema file names
/// Centralizes schema file references to avoid hardcoding throughout tests
/// </summary>
public static class SchemaConstants
{
    /// <summary>
    /// Schema for validating an array of Company objects
    /// </summary>
    public const string CompanyArray = "company-array-schema.json";
    
    /// <summary>
    /// Schema for validating a single Company object
    /// </summary>
    public const string Company = "company-schema.json";
}
