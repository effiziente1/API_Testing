# Running Tests in VS Code

## Prerequisites

### Install Required Extensions

When you open this project in VS Code for the first time, you'll see a notification:

**"This workspace has extension recommendations"**

Click **Install All** to automatically install:

- **C# Dev Kit** (ms-dotnettools.csdevkit) - Provides .NET testing support
- **C#** (ms-dotnettools.csharp) - C# language support
- **.NET Runtime** (ms-dotnettools.vscode-dotnet-runtime) - Runtime support
- **.NET Test Explorer** (formulahendry.dotnet-test-explorer) - Enhanced test discovery
- **NuGet Gallery** (patcx.vscode-nuget-gallery) - Package management

**Manual Installation:**
1. Press `Ctrl+Shift+X` (Windows/Linux) or `Cmd+Shift+X` (Mac)
2. Search for "C# Dev Kit"
3. Click Install

## Configuration

The project is configured with `.vscode/settings.json` to automatically use the runsettings file.

## Running Tests - Multiple Options

### Option 1: Using Test Explorer (Recommended)

1. Open the **Testing** view (flask icon in the left sidebar)
2. The test explorer will automatically discover all tests
3. Click the play button (▶️) next to any test or test class to run it
4. The runsettings file is automatically applied

### Option 2: Using Code Lens (Above Test Methods)

1. Open any test file (e.g., `LoginTests.cs`)
2. Look for "Run Test | Debug Test" links above each `[Test]` method
3. Click "Run Test" to execute the test with runsettings
4. Click "Debug Test" to debug with breakpoints

### Option 3: Using Terminal

Run all tests:
```bash
dotnet test --settings:ApiTestingDemo.Tests/test.runsettings
```

Run specific test file:
```bash
dotnet test --settings:ApiTestingDemo.Tests/test.runsettings --filter "FullyQualifiedName~LoginTests"
```

Run specific test:
```bash
dotnet test --settings:ApiTestingDemo.Tests/test.runsettings --filter "FullyQualifiedName~ValidCredentials_ReturnsToken"
```

### Option 4: Using Tasks (Ctrl+Shift+P)

1. Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
2. Type "Run Task"
3. Select "test" task
4. Tests will run with runsettings automatically

## Environment Setup

### Setting User Secrets (For Local Development)

1. Navigate to the test project directory:
```bash
cd ApiTestingDemo.Tests
```

2. Set your secrets:
```bash
dotnet user-secrets set "USER_PASSWORD" "your-password"
dotnet user-secrets set "USER_NAME" "your-username"
dotnet user-secrets set "USER_COMPANY" "your-company"
```

3. Verify secrets are set:
```bash
dotnet user-secrets list
```

### Or Use Environment Variables

Set environment variables in your terminal before running tests:

**Mac/Linux:**
```bash
export USER_PASSWORD="your-password"
export USER_NAME="your-username"
export USER_COMPANY="your-company"
```

**Windows PowerShell:**
```powershell
$env:USER_PASSWORD="your-password"
$env:USER_NAME="your-username"
$env:USER_COMPANY="your-company"
```

**Windows CMD:**
```cmd
set USER_PASSWORD=your-password
set USER_NAME=your-username
set USER_COMPANY=your-company
```

## Runsettings Configuration

The `test.runsettings` file contains:
- **AuthUrl**: API base URL (default: https://api.example.com)
- **DefaultTimeout**: Request timeout in milliseconds (default: 30000)
- **RetryAttempts**: Number of retry attempts (default: 3)

To change these values, edit `ApiTestingDemo.Tests/test.runsettings`

## Troubleshooting

### Tests can't find runsettings
- Make sure you've opened the **root folder** of the repository in VS Code
- Reload VS Code window: `Ctrl+Shift+P` → "Developer: Reload Window"

### Tests fail with null reference errors
- Ensure user secrets or environment variables are set
- Check that AuthUrl in runsettings points to a valid API

### Test Explorer doesn't show tests
- Open the Output panel and select ".NET Test Log" from the dropdown
- Check for any discovery errors
- Try rebuilding: `dotnet build`

## Debugging Tests

1. Set breakpoints in your test code
2. Right-click on the test in Test Explorer
3. Select "Debug Test"
4. Or use the "Debug Test" CodeLens link above the test method

The debugger will stop at your breakpoints with full variable inspection.
