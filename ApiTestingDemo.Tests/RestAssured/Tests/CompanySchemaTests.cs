using ApiTestingDemo.Tests.Shared;
using ApiTestingDemo.Tests.Shared.Clients;
using ApiTestingDemo.Tests.Shared.Helpers;

namespace ApiTestingDemo.Tests.RestAssured.Tests;

/// <summary>
/// Functional tests for Company API using RestAssured with schema validation
/// These tests verify API responses match the expected JSON schema
/// </summary>
public class CompanySchemaTests : TestBase
{
    private RestAssuredClient _restAssuredClient = null!;
    private string _baseUrl = string.Empty;

    [SetUp]
    public void Setup()
    {
        _baseUrl = TestContext.Parameters["AuthUrl"]!;
        _restAssuredClient = new RestAssuredClient(_baseUrl, AuthToken);
    }

    /// <summary>
    /// Test: Authenticated request with schema validation
    /// Combines authentication testing with schema validation
    /// </summary>
    [Test]
    public void GetCompanies_AuthenticatedRequestValidatesSchema()
    {
        // Arrange
        string schema = SchemaHelper.GetSchemaContent(SchemaConstants.CompanyArray);

        // Act & Assert
        // The RestAssuredClient already includes the auth token
        _restAssuredClient.CreateRequest()
            .When()
            .Get(_restAssuredClient.GetFullUrl("api/Server/1/Companies"))
            .Then()
            .StatusCode(200)
            .And()
            .MatchesJsonSchema(schema);

    }
}
