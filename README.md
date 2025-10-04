# API_Testing

Api testing sample (RestSharp, RestAssured)

## Configure User Secrets

```bash
cd ApiTestingDemo.Tests
dotnet user-secrets set "USER_COMPANY" "YourCompanyName"
dotnet user-secrets set "USER_NAME" "YourUsername"
dotnet user-secrets set "USER_PASSWORD" "YourPassword"
```

To see secrets run:

```bash
dotnet user-secrets list
```

## Run Tests

```bash
# Run all tests
dotnet test

# Run with settings
dotnet test --settings:.runsettings

# Run only RestSharp examples
dotnet test --filter "FullyQualifiedName~RestSharp.Examples"

# Run only RestAssured examples
dotnet test --filter "FullyQualifiedName~RestAssured.Examples"
```
