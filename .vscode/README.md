# VS Code Test Quick Reference

## 🎯 Quick Start

### 1. Install Required Extensions

When you open this project in VS Code, you'll see a notification to install recommended extensions. Click **Install All** to get:

- **C# Dev Kit** (Microsoft) - .NET development and testing
- **C#** (Microsoft) - C# language support
- **.NET Runtime** (Microsoft) - .NET runtime installation
- **.NET Test Explorer** - Enhanced test discovery
- **NuGet Gallery** - Package management

Or install manually from the Extensions view (Ctrl+Shift+X / Cmd+Shift+X)

### 2. Set User Secrets (One-time setup)
```bash
cd ApiTestingDemo.Tests
dotnet user-secrets set "USER_PASSWORD" "your-password"
dotnet user-secrets set "USER_NAME" "your-username"
dotnet user-secrets set "USER_COMPANY" "your-company"
```

### 3. Run Tests in VS Code

**Method 1: Test Explorer (Easiest)**
- Click Testing icon (🧪) in sidebar
- Click ▶️ next to test/class to run

**Method 2: CodeLens (In test files)**
- Open test file
- Click "Run Test" link above test method

**Method 3: Command Palette**
- `Cmd+Shift+P` (Mac) or `Ctrl+Shift+P` (Windows/Linux)
- Type "Run Task" → Select "test"

**Method 4: Terminal**
```bash
dotnet test --settings:ApiTestingDemo.Tests/test.runsettings
```

## 🔧 Configuration Files Created

| File | Purpose |
|------|---------|
| `.vscode/settings.json` | VS Code test settings with runsettings path |
| `.vscode/launch.json` | Debug configuration for tests |
| `.vscode/tasks.json` | Build and test tasks |
| `omnisharp.json` | OmniSharp configuration for runsettings |

All configurations automatically use `ApiTestingDemo.Tests/test.runsettings`

## 📝 Test Parameters (in test.runsettings)

```xml
<Parameter name="AuthUrl" value="https://api.example.com" />
<Parameter name="DefaultTimeout" value="30000" />
<Parameter name="RetryAttempts" value="3" />
```

Edit `ApiTestingDemo.Tests/test.runsettings` to change these values.

## 🐛 Debug Tests

1. Set breakpoint in test code
2. Right-click test in Test Explorer → "Debug Test"
   OR
3. Click "Debug Test" CodeLens link above test method

## ✅ Everything is Already Configured!

The project includes:
- ✅ VS Code settings for runsettings
- ✅ Debug configurations
- ✅ Build and test tasks
- ✅ Test Explorer integration
- ✅ Allure reporting

Just open the folder in VS Code and start testing!
