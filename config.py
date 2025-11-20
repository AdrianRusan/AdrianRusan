"""
Configuration file for Branch Monitor
Customize these settings as needed
"""

# Organization settings
ORG_NAME = 'bono-ro'
RECIPIENT_EMAIL = 'adrian@bono.ro'
DAYS_THRESHOLD = 30  # Number of days before a branch is considered stale

# Protected branches (case-insensitive)
# These branches will never be flagged as stale
PROTECTED_BRANCHES = {
    'main',
    'master',
    'staging',
    'development',
    'develop',
    'prod',
    'production'
}

# Email settings (for local testing)
# For GitHub Actions, these are set via secrets
SMTP_SERVER = 'smtp.gmail.com'
SMTP_PORT = 587

# Add more custom settings here if needed
