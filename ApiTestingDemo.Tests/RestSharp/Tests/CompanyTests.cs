using ApiTestingDemo.Tests.Shared;
using ApiTestingDemo.Tests.Shared.Clients;
using ApiTestingDemo.Tests.Shared.Models;
using FluentAssertions;

namespace ApiTestingDemo.Tests.RestSharp.Tests;

/// <summary>
/// Functional tests for Company API endpoints using RestSharp
/// These tests verify the actual business functionality of the API
/// </summary>
[Category("Company")]
[Category("API")]
public class CompanyTests : TestBase
{
    private IApiClient _apiClient = null!;
    private string _baseUrl = string.Empty;

    [SetUp]
    public void Setup()
    {
        _baseUrl = TestContext.Parameters["AuthUrl"]!;
        _apiClient = new RestSharpClient(_baseUrl, AuthToken);
    }

    /// <summary>
    /// Test: GET companies by valid server ID returns data
    /// </summary>
    [Test]
    [Category("Smoke")]
    public async Task GetCompanies_WithValidServerId_ReturnsCompanies()
    {
        // Arrange
        var serverId = 1;

        // Act
        var response = await _apiClient.GetAsync<Company[]>($"api/Server/{serverId}/Companies");

        // Assert
        response.IsSuccess.Should().BeTrue("request should succeed");
        response.StatusCode.Should().Be(200, "should return OK status");
        response.Data.Should().NotBeNull("response should contain data");
        response.Data.Should().NotBeEmpty("server 1 should have companies");

        // Verify all companies belong to the requested server
        response.Data.Should().AllSatisfy(company =>
        {
            company.ServerId.Should().Be(serverId, "all companies should belong to requested server");
        });
    }

    /// <summary>
    /// Test: GET companies with invalid server ID returns empty array
    /// </summary>
    /// <param name="serverId">Invalid server ID to test</param>
    [TestCase(0, Description = "Zero server ID")]
    [TestCase(999, Description = "Non-existent server ID")]
    public async Task GetCompanies_WithInvalidServerId_ReturnsEmptyArray(int serverId)
    {
        // Act
        var response = await _apiClient.GetAsync<Company[]>($"api/Server/{serverId}/Companies");

        // Assert
        response.IsSuccess.Should().BeTrue("request should complete successfully");
        response.StatusCode.Should().Be(200, "should return OK status");
        response.Data.Should().NotBeNull("response should contain data");
        response.Data.Should().BeEmpty($"server {serverId} should have no companies");
    }

    /// <summary>
    /// Test: Request without authentication fails with 401
    /// </summary>
    [Test]
    public async Task GetCompanies_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var unauthenticatedClient = new RestSharpClient(_baseUrl);

        // Act
        var response = await unauthenticatedClient.GetAsync<Company[]>("api/Server/1/Companies");

        // Assert
        response.IsSuccess.Should().BeFalse("unauthenticated request should fail");
        response.StatusCode.Should().Be(401, "should return Unauthorized status");
    }

    /// <summary>
    /// Test: Verify company data structure and properties
    /// </summary>
    [Test]
    public async Task GetCompanies_ReturnsValidCompanyStructure()
    {
        // Act
        var response = await _apiClient.GetAsync<Company[]>("api/Server/1/Companies");

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeEmpty();

        // Verify first company has all required properties with valid values
        var firstCompany = response.Data!.First();

        firstCompany.Id.Should().BeGreaterThan(0, "company ID should be positive");
        firstCompany.ServerId.Should().BeGreaterThan(0, "server ID should be positive");
        firstCompany.Key.Should().BeGreaterThan(0, "company key should be positive");
        firstCompany.Name.Should().NotBeNullOrEmpty("company should have a name");
        firstCompany.DataBase.Should().NotBeNull("database field should be present");
    }

    /// <summary>
    /// Test: Verify response time is acceptable
    /// </summary>
    [Test]
    public async Task GetCompanies_ReturnsWithinAcceptableTime()
    {
        // Arrange
        var maxAcceptableMs = int.Parse(
            Environment.GetEnvironmentVariable("MAX_ACCEPTABLE_MS")
            ?? TestContext.Parameters["MaxAcceptableMs"]
            ?? "5000"
        );

        // Act
        var startTime = DateTime.UtcNow;
        var response = await _apiClient.GetAsync<Company[]>("api/Server/1/Companies");
        var duration = DateTime.UtcNow - startTime;

        // Assert
        response.IsSuccess.Should().BeTrue();
        duration.TotalMilliseconds.Should().BeLessThan(maxAcceptableMs,
            $"request should complete within {maxAcceptableMs}ms");
    }
}
