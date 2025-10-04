# API_Testing

[![Test Status](https://github.com/effiziente1/API_Testing/actions/workflows/test-and-report.yml/badge.svg)](https://github.com/effiziente1/API_Testing/actions/workflows/test-and-report.yml)
[![Allure Report](https://img.shields.io/badge/Allure%20Report-📊-blue)](https://effiziente1.github.io/API_Testing/)

API testing sample using RestSharp and RestAssured.Net with Allure reporting

## 📊 Test Reports

**Latest Allure Report:** [https://effiziente1.github.io/API_Testing/](https://effiziente1.github.io/API_Testing/)

View interactive test results with trends, history, and detailed test execution reports.

## 📚 Documentation

- **[VS Code Testing Guide](VSCODE_TESTING.md)** - How to run and debug tests in VS Code
- **[GitHub Actions Setup](GITHUB_SETUP.md)** - CI/CD pipeline configuration
- **[Quick Reference](.vscode/README.md)** - VS Code quick start guide

## 🚀 Quick Start

### 1. Configure User Secrets

```bash
cd ApiTestingDemo.Tests
dotnet user-secrets set "USER_COMPANY" "YourCompanyName"
dotnet user-secrets set "USER_NAME" "YourUsername"
dotnet user-secrets set "USER_PASSWORD" "YourPassword"
```

To see secrets:
```bash
dotnet user-secrets list
```

### 2. Run Tests

**With runsettings (Recommended):**
```bash
dotnet test --settings:ApiTestingDemo.Tests/test.runsettings
```

**Filter by test class:**
```bash
dotnet test --settings:ApiTestingDemo.Tests/test.runsettings --filter "FullyQualifiedName~LoginTests"
```

**Filter by library:**
```bash
# RestSharp tests only
dotnet test --filter "FullyQualifiedName~RestSharp"

# RestAssured tests only
dotnet test --filter "FullyQualifiedName~RestAssured"
```

## 🧪 VS Code Testing

### First-time Setup
1. Open the project in VS Code
2. Click **Install All** when prompted to install recommended extensions
3. Configure user secrets (see above)

### Running Tests
Open the project in VS Code and use:
- **Test Explorer** - Click the testing icon (🧪) in the sidebar
- **CodeLens** - Click "Run Test" above any test method
- **Tasks** - `Cmd/Ctrl+Shift+P` → "Run Task" → "test"

See [VSCODE_TESTING.md](VSCODE_TESTING.md) for detailed instructions.

## 📊 Allure Reports

Reports are generated automatically in `allure-results/` directory.

**Generate HTML report locally:**
```bash
allure generate allure-results -o allure-report --clean
allure open allure-report
```

**CI/CD:** GitHub Actions automatically generates and publishes reports. See [GITHUB_SETUP.md](GITHUB_SETUP.md).

## 🏗️ Project Structure

```
ApiTestingDemo.Tests/
├── RestSharp/Tests/          # RestSharp test examples
├── RestAssured/Tests/        # RestAssured.Net test examples
├── Shared/
│   ├── Clients/             # API client implementations
│   ├── Helpers/             # Utility classes
│   ├── Models/              # Data models
│   └── TestBase.cs          # Base test class
├── Schemas/                 # JSON schemas for validation
└── test.runsettings         # Test configuration
```

## ⚙️ Configuration

**Test Settings** (`test.runsettings`):
- `AuthUrl` - API base URL
- `DefaultTimeout` - Request timeout (ms)
- `RetryAttempts` - Number of retry attempts

**Secrets** (User Secrets or Environment Variables):
- `USER_COMPANY` - Company name
- `USER_NAME` - Username
- `USER_PASSWORD` - Password
