# 🔍 Organization Branch Monitor

Automatically monitor and report on stale branches across all repositories in your GitHub organization.

## 📋 Overview

This tool scans all repositories in the `bono-ro` GitHub organization and sends weekly email reports about branches that are **older than 30 days** and need cleanup. It helps keep your repositories clean and organized by identifying branches that developers may have forgotten to delete.

## ✨ Features

- ✅ Scans all repositories in your organization (30+ repos supported)
- ✅ Identifies branches older than 30 days
- ✅ Automatically excludes protected branches (`main`, `master`, `staging`, `development`)
- ✅ Sends beautiful HTML email reports weekly
- ✅ Runs automatically via GitHub Actions (no server needed)
- ✅ Shows branch age, last commit, and author information
- ✅ Direct links to each stale branch for easy deletion
- ✅ Skips archived repositories

## 🚀 Quick Start

### 1. Create GitHub Personal Access Token

1. Go to GitHub Settings → Developer settings → [Personal Access Tokens](https://github.com/settings/tokens)
2. Click **"Generate new token (classic)"**
3. Give it a descriptive name: `Branch Monitor Token`
4. Select the following permissions:
   - ✅ `repo` (Full control of private repositories)
   - ✅ `read:org` (Read org and team membership)
5. Click **"Generate token"**
6. **Copy the token immediately** (you won't see it again!)

### 2. Set Up Email Credentials

**For Gmail:**
1. Enable 2-factor authentication on your Google account
2. Go to [Google App Passwords](https://myaccount.google.com/apppasswords)
3. Create a new app password named "Branch Monitor"
4. Copy the 16-character password

**For Other Email Providers:**
- Use your SMTP server settings
- Create an app-specific password if available

### 3. Configure GitHub Secrets

Go to your repository's Settings → Secrets and variables → Actions → New repository secret

Add the following secrets:

| Secret Name | Description | Example Value |
|------------|-------------|---------------|
| `ORG_GITHUB_TOKEN` | Your GitHub PAT from step 1 | `ghp_xxxxxxxxxxxx` |
| `SENDER_EMAIL` | Email address to send from | `your-email@gmail.com` |
| `SENDER_PASSWORD` | App password from step 2 | `abcd efgh ijkl mnop` |
| `SMTP_SERVER` | SMTP server address | `smtp.gmail.com` |
| `SMTP_PORT` | SMTP port number | `587` |

### 4. Enable GitHub Actions

1. Go to your repository's **Actions** tab
2. If prompted, click **"I understand my workflows, go ahead and enable them"**
3. The workflow will run automatically every Monday at 9:00 AM UTC

### 5. Test the Workflow (Optional)

To test immediately without waiting for the schedule:

1. Go to **Actions** tab
2. Click **"Organization Branch Monitor"** workflow
3. Click **"Run workflow"** dropdown
4. Click the green **"Run workflow"** button

## 📧 Email Report

The email report includes:

- **Summary**: Total stale branches and affected repositories
- **Per-Repository Tables**: Organized by repository with branch details
- **Branch Information**:
  - Branch name with direct link
  - Days since last commit
  - Last commit date
  - Last author
  - Last commit message preview

### Sample Email

```
🚨 Stale Branch Report

⚠️ Action Required: The following branches are older than 30 days

Total Stale Branches: 15
Repositories Affected: 5

📂 my-web-app (6 stale branches)
┌────────────────┬──────────┬──────────────┬─────────────┐
│ Branch Name    │ Days Old │ Last Commit  │ Last Author │
├────────────────┼──────────┼──────────────┼─────────────┤
│ feature/old-ui │ 45 days  │ 2024-10-01   │ John Doe    │
│ bugfix/login   │ 38 days  │ 2024-10-08   │ Jane Smith  │
└────────────────┴──────────┴──────────────┴─────────────┘
```

## 🛠️ Local Testing

To test the script locally before deploying:

### 1. Clone and Install

```bash
git clone https://github.com/AdrianRusan/AdrianRusan.git
cd AdrianRusan
pip install -r requirements.txt
```

### 2. Set Up Environment

```bash
cp .env.example .env
# Edit .env with your actual values
nano .env
```

### 3. Run the Script

```bash
python check_stale_branches.py
```

## ⚙️ Configuration

### Change Schedule

Edit `.github/workflows/branch-monitor.yml`:

```yaml
on:
  schedule:
    # Daily at 9 AM UTC
    - cron: '0 9 * * *'

    # Every Monday and Friday at 9 AM UTC
    - cron: '0 9 * * 1,5'
```

### Change Days Threshold

In the workflow file, change `DAYS_THRESHOLD`:

```yaml
env:
  DAYS_THRESHOLD: '45'  # Flag branches older than 45 days
```

### Add More Protected Branches

Edit `check_stale_branches.py`:

```python
PROTECTED_BRANCHES = {
    'main', 'master', 'staging', 'development',
    'hotfix', 'release'  # Add your custom branches here
}
```

## 📁 Project Structure

```
.
├── .github/
│   └── workflows/
│       └── branch-monitor.yml    # GitHub Actions workflow
├── check_stale_branches.py       # Main Python script
├── config.py                     # Configuration settings
├── requirements.txt              # Python dependencies
├── .env.example                  # Environment variables template
├── .gitignore                    # Git ignore file
└── README.md                     # This file
```

## 🔒 Security Notes

- **Never commit** your `.env` file or expose your tokens
- Use GitHub Secrets for all sensitive data in Actions
- Use app-specific passwords, not your main account password
- Rotate tokens periodically
- The script only has **read access** to repositories (won't modify anything)

## 🐛 Troubleshooting

### Workflow Failed

1. Check the Actions tab for error logs
2. Verify all GitHub Secrets are set correctly
3. Ensure your GitHub token has the right permissions

### No Email Received

1. Check your spam folder
2. Verify SMTP settings are correct
3. For Gmail, ensure app password is used (not regular password)
4. Check the Actions logs for error messages

### Token Permission Errors

Ensure your GitHub token has:
- `repo` scope
- `read:org` scope

## 📊 What Gets Monitored

- ✅ All non-archived repositories in `bono-ro` organization
- ✅ All branches except: `main`, `master`, `staging`, `development`, `develop`, `prod`, `production`
- ✅ Branches with last commit older than 30 days

## 🤝 Contributing

Feel free to submit issues or pull requests if you have suggestions for improvements!

## 📝 License

This project is open source and available for personal and commercial use.

## 👤 Author

**Adrian Rusan**
- Email: adrian@bono.ro
- GitHub: [@AdrianRusan](https://github.com/AdrianRusan)

---

**Questions?** Open an issue or contact adrian@bono.ro
