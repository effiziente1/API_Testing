using Allure.NUnit.Attributes;
using ApiTestingDemo.Tests.Shared;
using ApiTestingDemo.Tests.Shared.Clients;
using NHamcrest.Core;
using System;

namespace ApiTestingDemo.Tests.RestAssured.Tests;

/// <summary>
/// API tests for the Server resource focusing on POST /api/Server
/// Generated from Swagger: https://effizienteauthdemo.azurewebsites.net/swagger/v1/swagger.json
/// </summary>
[AllureParentSuite("Server")]
[Category("Server")]
public class ServerApiTests : TestBase
{
    private RestAssuredClient _restAssuredClient = null!;
    private string _baseUrl = string.Empty;
    private Random _random = null!;

    [SetUp]
    public void Setup()
    {
        _baseUrl = TestContext.Parameters["AuthUrl"] ?? Configuration["AuthUrl"]!;
        _restAssuredClient = new RestAssuredClient(_baseUrl, AuthToken);
        _random = new Random();
    }

    /// <summary>
    /// Positive: Create server with valid data -> 201 Created
    /// Validates all fields in the response schema and that values match request
    /// Schema constraints observed: Key (int 1..999999), Name (string max 150), Url (string max 200), Active (bool)
    /// </summary>
    [Test]
    public void PostServer_ValidData_ReturnsCreatedWithMatchingFields()
    {
        // Arrange - generate unique test data within schema constraints
        var key = _random.Next(1, 1000000); // maximum is exclusive
        var suffix = _random.Next(1000, 9999);
        var name = $"RA-Server-{suffix}"; // maxLength 150
        var url = $"https://example{suffix}.contoso.com"; // maxLength 200
        var requestBody = new
        {
            Key = key,
            Name = name,
            Url = url,
            Active = true
        };

        // Act & Assert
        _restAssuredClient.CreateRequest()
            .ContentType("application/json")
            .Body(requestBody)
        .When()
            .Post(_restAssuredClient.GetFullUrl("/api/Server"))
        .Then()
            .StatusCode(201)
            // Validate all response properties defined in schema
            .Body("$.Id", NHamcrest.Is.GreaterThan(0))
            .Body("$.Key", NHamcrest.Is.EqualTo(requestBody.Key))
            .Body("$.Name", NHamcrest.Is.EqualTo(requestBody.Name))
            .Body("$.Url", NHamcrest.Is.EqualTo(requestBody.Url))
            .Body("$.Active", NHamcrest.Is.EqualTo(requestBody.Active));
    }

    /// <summary>
    /// Negative: Missing required field (e.g., Name) -> 400 Bad Request (as per typical validation)
    /// If the API returns a different documented error, adjust accordingly. Swagger does not explicitly list 400 for POST, so we still include this negative assuming validation.
    /// </summary>
    [Test]
    public void PostServer_MissingRequiredName_ReturnsBadRequest()
    {
        var key = _random.Next(1, 1000000);
        var suffix = _random.Next(1000, 9999);
        var url = $"https://example{suffix}.contoso.com";

        var invalidBody = new
        {
            Key = key,
            // Name missing
            Url = url,
            Active = true
        };

        _restAssuredClient.CreateRequest()
            .ContentType("application/json")
            .Body(invalidBody)
        .When()
            .Post(_restAssuredClient.GetFullUrl("/api/Server"))
        .Then()
            .StatusCode(400);
    }

    /// <summary>
    /// Negative: Invalid authentication -> 401 Unauthorized
    /// Global security scheme is Bearer; verify unauthorized without token
    /// </summary>
    [Test]
    public void PostServer_WithoutAuth_ReturnsUnauthorized()
    {
        var key = _random.Next(1, 1000000);
        var suffix = _random.Next(1000, 9999);
        var name = $"RA-Server-{suffix}";
        var url = $"https://example{suffix}.contoso.com";

        var body = new { Key = key, Name = name, Url = url, Active = true };

        new RestAssuredClient(_baseUrl) // no token
            .CreateRequest()
            .ContentType("application/json")
            .Body(body)
        .When()
            .Post(_restAssuredClient.GetFullUrl("/api/Server"))
        .Then()
            .StatusCode(401);
    }

    /// <summary>
    /// Negative: Invalid data - Key below minimum -> 400 Bad Request
    /// Constraint: Key minimum is 1
    /// </summary>
    [Test]
    public void PostServer_InvalidKeyBelowMinimum_ReturnsBadRequest()
    {
        var suffix = _random.Next(1000, 9999);
        var body = new
        {
            Key = 0, // below minimum 1
            Name = $"RA-Server-{suffix}",
            Url = $"https://example{suffix}.contoso.com",
            Active = true
        };

        _restAssuredClient.CreateRequest()
            .ContentType("application/json")
            .Body(body)
        .When()
            .Post(_restAssuredClient.GetFullUrl("/api/Server"))
        .Then()
            .StatusCode(400);
    }

    /// <summary>
    /// Positive: Get all servers -> 200 OK
    /// Validates array of Server objects and required fields per schema
    /// Schema: Server { Id:int?, Key:int[1..999999], Name:string(max150), Url:string(max200), Active:bool }
    /// </summary>
    [Test]
    public void GetServers_WithValidAuth_ReturnsOkAndValidSchema()
    {
        _restAssuredClient.CreateRequest()
            .Accept("application/json")
        .When()
            .Get(_restAssuredClient.GetFullUrl("/api/Server"))
        .Then()
            .StatusCode(200)
            // Validate at least one item may exist (if none, length() could be 0).
            // We assert types of first element when present; APIs may return empty list, so status-only check is acceptable if empty.
            .Body("$[0].Key", NHamcrest.Is.GreaterThan(0))
            .Body("$[0].Name", NHamcrest.Is.Not(NHamcrest.Is.EqualTo<string?>(null)))
            .Body("$[0].Url", NHamcrest.Is.Not(NHamcrest.Is.EqualTo<string?>(null)))
            .Body("$[0].Active", NHamcrest.Is.Anything());
    }

    /// <summary>
    /// Negative: Unauthorized access -> 401 when no token
    /// Global security requires Bearer token
    /// </summary>
    [Test]
    [Category("Negative")]
    public void GetServers_WithoutAuth_ReturnsUnauthorized()
    {
        new RestAssuredClient(_baseUrl) // no token
            .CreateRequest()
            .Accept("application/json")
        .When()
            .Get(_restAssuredClient.GetFullUrl("/api/Server"))
        .Then()
            .StatusCode(401);
    }

}
