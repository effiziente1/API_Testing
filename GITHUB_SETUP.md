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
| `AUTH_URL` | API base URL for authentication endpoint |

### 2. GitHub Pages Configuration

1. Go to **Settings** → **Pages**
2. Under **Build and deployment**:
   - **Source**: Deploy from a branch
   - **Branch**: `gh-pages` / `root`
3. Click **Save**

**Note**: The `gh-pages` branch will be automatically created on the first workflow run.

### 3. Workflow Triggers

The pipeline runs on:
- **Push to main branch**: Automatically runs when code is pushed to main
- **Manual trigger**: Use "Run workflow" button in Actions tab
- **Scheduled**: Daily at midnight UTC

## Accessing Allure Reports

After the workflow completes successfully, you can access the reports in two ways:

### Option 1: Download as Artifact (No server required)
1. Go to **Actions** tab in your repository
2. Click on the completed workflow run
3. Scroll down to **Artifacts** section
4. Download **allure-report.zip**
5. Extract the ZIP file locally
6. Open `index.html` in your browser to view the report

### Option 2: View on GitHub Pages (Online)
1. Navigate to **Settings** → **Pages** to find your GitHub Pages URL
2. The Allure report will be available at: `https://<username>.github.io/<repository>/`
3. Reports include test history and trends from previous runs

**Note**: Both options show the same HTML report with full interactivity, trends, and history.

## Troubleshooting

- **First Run**: The initial run may not show trends (no history yet)
- **Secrets**: Ensure all four secrets are correctly configured (USER_PASSWORD, USER_NAME, USER_COMPANY, AUTH_URL)
- **Pages**: Wait a few minutes after workflow completion for Pages to deploy
- **Permissions**: The workflow uses `GITHUB_TOKEN` with write permissions
- **Test Settings**: Default values in test.runsettings are used if AuthUrl is not overridden
