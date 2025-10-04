using System.Reflection;

namespace ApiTestingDemo.Tests.Shared.Helpers;

/// <summary>
/// Helper class for loading JSON schema files
/// Schemas are used for validating API responses in RestAssured tests
/// </summary>
public static class SchemaHelper
{
    private static readonly string SchemasDirectory = GetSchemasDirectory();

    /// <summary>
    /// Gets the Schemas directory path from the assembly output location
    /// </summary>
    private static string GetSchemasDirectory()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = Path.GetDirectoryName(assembly.Location)!;
        return Path.Combine(assemblyLocation, "Schemas");
    }

    /// <summary>
    /// Reads and returns the content of a JSON schema file
    /// </summary>
    /// <param name="schemaFileName">Name of the schema file (e.g., "company-schema.json")</param>
    /// <returns>Schema content as string</returns>
    /// <exception cref="FileNotFoundException">Thrown when schema file doesn't exist</exception>
    public static string GetSchemaContent(string schemaFileName)
    {
        var schemaPath = Path.Combine(SchemasDirectory, schemaFileName);
        
        if (!File.Exists(schemaPath))
        {
            throw new FileNotFoundException($"Schema file not found: {schemaPath}");
        }
        
        return File.ReadAllText(schemaPath);
    }

    /// <summary>
    /// Gets the full path to a schema file
    /// </summary>
    /// <param name="schemaFileName">Name of the schema file</param>
    /// <returns>Full file path</returns>
    public static string GetSchemaPath(string schemaFileName)
    {
        return Path.Combine(SchemasDirectory, schemaFileName);
    }
}
