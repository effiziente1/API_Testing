# GitHub Repository Setup Guide

## Required Configuration

### 1. GitHub Secrets Setup

Add the following secrets to your GitHub repository:

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret** for each:

| Secret Name | Description |
|-------------|-------------|
| `USER_PASSWORD` | User password for API authentication |
| `USER_NAME` | Username for API authentication |
| `USER_COMPANY` | Company name for API testing |

### 2. Configure API URL (Optional)

If you need to override the default API URL from `test.runsettings`:

1. Edit `ApiTestingDemo.Tests/test.runsettings`
2. Update the `AuthUrl` parameter value:
   ```xml
   <Parameter name="AuthUrl" value="https://your-api-url.com" />
   ```

**Note**: The AuthUrl is configured in runsettings, not as a GitHub secret.

### 3. GitHub Pages Configuration

1. Go to **Settings** → **Pages**
2. Under **Build and deployment**:
   - **Source**: Deploy from a branch
   - **Branch**: `gh-pages` / `root`
3. Click **Save**

**Note**: The `gh-pages` branch will be automatically created on the first workflow run.

### 4. Workflow Triggers

The pipeline runs on:
- **Push to main branch**: Automatically runs when code is pushed to main
- **Manual trigger**: Use "Run workflow" button in Actions tab
- **Scheduled**: Daily at 1:00 AM CST (Mexico Central Time)

**Note**: The workflow runs in a single job for efficiency (checkout code only once).

## Workflow Steps

The pipeline automatically:
1. ✅ Runs .NET tests with Allure reporting
2. ✅ Preserves test history from previous runs
3. ✅ Generates interactive HTML report
4. ✅ Uploads report folder as artifact (GitHub zips automatically)
5. ✅ Publishes report to GitHub Pages

**Java Requirement**: Yes, Java is required for Allure Report generation.
- The workflow automatically installs Java 17
- Allure CLI is installed via npm (`allure-commandline` package)
- Java is needed because Allure Report is built on Java (no pure Node.js version exists)

## Accessing Allure Reports

After the workflow completes successfully, you can access the reports in two ways:

### Option 1: Download Single-File Report (✅ Download & Open)
1. Go to **Actions** tab in your repository
2. Click on the completed workflow run
3. Scroll down to **Artifacts** section
4. Download **allure-single-file-report**
5. Double-click the downloaded `index.html` file

**✅ Works directly** - No server needed! This is a self-contained HTML file with all resources embedded.

**Note**: This single-file version has the same content but may be slightly slower to load than the full version.

### Option 2: Download Full Report (Requires server)
1. Download **allure-report** artifact
2. Extract and run: `python -m http.server 8080`
3. Open: http://localhost:8080

### Option 3: View on GitHub Pages (Online - Recommended)
1. Navigate to **Settings** → **Pages** to find your GitHub Pages URL
2. The Allure report will be available at: `https://<username>.github.io/<repository>/`
3. Reports include test history and trends from previous runs

**Note**: Both options show the same HTML report with full interactivity, trends, and history.

## Troubleshooting

- **First Run**: The initial run may not show trends (no history yet)
- **Secrets**: Ensure all three secrets are correctly configured (USER_PASSWORD, USER_NAME, USER_COMPANY)
- **Pages**: Wait a few minutes after workflow completion for Pages to deploy
- **Permissions**: The workflow uses `GITHUB_TOKEN` with write permissions
- **API URL**: Configure the AuthUrl in `test.runsettings` file, not as a secret
