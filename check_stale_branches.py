#!/usr/bin/env python3
"""
Organization Branch Monitor
Scans all repositories in a GitHub organization and identifies stale branches.
"""

import os
import sys
from datetime import datetime, timezone, timedelta
from typing import List, Dict
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart
from github import Github, GithubException


class BranchMonitor:
    """Monitor branches across an organization's repositories."""

    # Protected branches that should never be flagged
    PROTECTED_BRANCHES = {'main', 'master', 'staging', 'development', 'develop', 'prod', 'production'}

    def __init__(self, github_token: str, org_name: str, days_threshold: int = 30):
        """
        Initialize the branch monitor.

        Args:
            github_token: GitHub personal access token
            org_name: GitHub organization name
            days_threshold: Number of days before a branch is considered stale
        """
        self.github = Github(github_token)
        self.org_name = org_name
        self.days_threshold = days_threshold
        self.stale_branches = []

    def scan_organization(self) -> List[Dict]:
        """
        Scan all repositories in the organization for stale branches.

        Returns:
            List of dictionaries containing stale branch information
        """
        print(f"🔍 Scanning organization: {self.org_name}")

        try:
            org = self.github.get_organization(self.org_name)
            repos = org.get_repos(type='all')

            repo_count = 0
            for repo in repos:
                repo_count += 1
                if repo.archived:
                    print(f"⏭️  Skipping archived repo: {repo.name}")
                    continue

                print(f"📂 Checking repository: {repo.name}")
                self._scan_repository(repo)

            print(f"\n✅ Scanned {repo_count} repositories")
            print(f"🚨 Found {len(self.stale_branches)} stale branches")

        except GithubException as e:
            print(f"❌ Error accessing organization: {e}")
            sys.exit(1)

        return self.stale_branches

    def _scan_repository(self, repo):
        """Scan a single repository for stale branches."""
        try:
            branches = repo.get_branches()
            cutoff_date = datetime.now(timezone.utc) - timedelta(days=self.days_threshold)

            for branch in branches:
                # Skip protected branches
                if branch.name.lower() in self.PROTECTED_BRANCHES:
                    continue

                # Get last commit date
                commit = branch.commit
                commit_date = commit.commit.author.date

                # Check if branch is stale
                if commit_date < cutoff_date:
                    days_old = (datetime.now(timezone.utc) - commit_date).days

                    self.stale_branches.append({
                        'repo_name': repo.name,
                        'repo_url': repo.html_url,
                        'branch_name': branch.name,
                        'branch_url': f"{repo.html_url}/tree/{branch.name}",
                        'last_commit_date': commit_date.strftime('%Y-%m-%d'),
                        'days_old': days_old,
                        'last_author': commit.commit.author.name,
                        'last_author_email': commit.commit.author.email,
                        'last_commit_message': commit.commit.message.split('\n')[0][:100]
                    })

                    print(f"  ⚠️  Stale branch found: {branch.name} ({days_old} days old)")

        except GithubException as e:
            print(f"  ⚠️  Error scanning {repo.name}: {e}")

    def generate_html_report(self) -> str:
        """Generate an HTML email report."""
        if not self.stale_branches:
            return """
            <html>
            <body>
                <h2>✅ No Stale Branches Found</h2>
                <p>All branches in your organization are up to date!</p>
            </body>
            </html>
            """

        # Group branches by repository
        repos_dict = {}
        for branch in self.stale_branches:
            repo_name = branch['repo_name']
            if repo_name not in repos_dict:
                repos_dict[repo_name] = []
            repos_dict[repo_name].append(branch)

        # Sort repositories by number of stale branches
        sorted_repos = sorted(repos_dict.items(), key=lambda x: len(x[1]), reverse=True)

        html = """
        <html>
        <head>
            <style>
                body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
                h1 { color: #d73a49; }
                h2 { color: #0366d6; margin-top: 30px; }
                table { border-collapse: collapse; width: 100%; margin-top: 10px; }
                th { background-color: #f6f8fa; padding: 12px; text-align: left; border: 1px solid #ddd; }
                td { padding: 10px; border: 1px solid #ddd; }
                tr:nth-child(even) { background-color: #f9f9f9; }
                .summary { background-color: #fff3cd; padding: 15px; border-radius: 5px; margin-bottom: 20px; }
                .warning { color: #856404; }
                a { color: #0366d6; text-decoration: none; }
                a:hover { text-decoration: underline; }
                .footer { margin-top: 30px; padding-top: 20px; border-top: 1px solid #ddd; color: #666; font-size: 12px; }
            </style>
        </head>
        <body>
            <h1>🚨 Stale Branch Report</h1>
            <div class="summary">
                <p class="warning"><strong>⚠️ Action Required:</strong> The following branches are older than {days} days and should be reviewed for deletion.</p>
                <p><strong>Total Stale Branches:</strong> {total_branches}</p>
                <p><strong>Repositories Affected:</strong> {total_repos}</p>
                <p><strong>Organization:</strong> <a href="https://github.com/{org_name}">{org_name}</a></p>
            </div>
        """.format(
            days=self.days_threshold,
            total_branches=len(self.stale_branches),
            total_repos=len(repos_dict),
            org_name=self.org_name
        )

        for repo_name, branches in sorted_repos:
            repo_url = branches[0]['repo_url']
            html += f"""
            <h2>📂 <a href="{repo_url}">{repo_name}</a></h2>
            <p><strong>{len(branches)} stale branch(es)</strong></p>
            <table>
                <tr>
                    <th>Branch Name</th>
                    <th>Days Old</th>
                    <th>Last Commit Date</th>
                    <th>Last Author</th>
                    <th>Last Commit</th>
                </tr>
            """

            for branch in sorted(branches, key=lambda x: x['days_old'], reverse=True):
                html += f"""
                <tr>
                    <td><a href="{branch['branch_url']}">{branch['branch_name']}</a></td>
                    <td>{branch['days_old']} days</td>
                    <td>{branch['last_commit_date']}</td>
                    <td>{branch['last_author']}</td>
                    <td>{branch['last_commit_message']}</td>
                </tr>
                """

            html += "</table>"

        html += """
            <div class="footer">
                <p>This is an automated report generated by Organization Branch Monitor.</p>
                <p>To delete a branch, visit the repository and run: <code>git push origin --delete &lt;branch-name&gt;</code></p>
            </div>
        </body>
        </html>
        """

        return html


def send_email(subject: str, html_content: str, recipient: str,
               smtp_server: str, smtp_port: int, sender_email: str, sender_password: str):
    """
    Send an email with the report.

    Args:
        subject: Email subject
        html_content: HTML content of the email
        recipient: Recipient email address
        smtp_server: SMTP server address
        smtp_port: SMTP server port
        sender_email: Sender email address
        sender_password: Sender email password/app password
    """
    print(f"\n📧 Sending email to: {recipient}")

    try:
        msg = MIMEMultipart('alternative')
        msg['Subject'] = subject
        msg['From'] = sender_email
        msg['To'] = recipient

        html_part = MIMEText(html_content, 'html')
        msg.attach(html_part)

        with smtplib.SMTP(smtp_server, smtp_port) as server:
            server.starttls()
            server.login(sender_email, sender_password)
            server.send_message(msg)

        print("✅ Email sent successfully!")

    except Exception as e:
        print(f"❌ Failed to send email: {e}")
        sys.exit(1)


def main():
    """Main execution function."""
    print("=" * 60)
    print("🔍 Organization Branch Monitor")
    print("=" * 60)

    # Load environment variables
    github_token = os.getenv('GITHUB_TOKEN')
    org_name = os.getenv('ORG_NAME', 'bono-ro')
    recipient_email = os.getenv('RECIPIENT_EMAIL', 'adrian@bono.ro')
    days_threshold = int(os.getenv('DAYS_THRESHOLD', '30'))

    # Email configuration
    smtp_server = os.getenv('SMTP_SERVER', 'smtp.gmail.com')
    smtp_port = int(os.getenv('SMTP_PORT', '587'))
    sender_email = os.getenv('SENDER_EMAIL')
    sender_password = os.getenv('SENDER_PASSWORD')

    # Validate required variables
    if not github_token:
        print("❌ Error: GITHUB_TOKEN environment variable is required")
        sys.exit(1)

    if not sender_email or not sender_password:
        print("❌ Error: SENDER_EMAIL and SENDER_PASSWORD are required for email notifications")
        sys.exit(1)

    # Initialize monitor and scan
    monitor = BranchMonitor(github_token, org_name, days_threshold)
    stale_branches = monitor.scan_organization()

    # Generate report
    html_report = monitor.generate_html_report()

    # Send email
    subject = f"🚨 Stale Branch Report - {org_name} - {len(stale_branches)} branches need attention"
    if not stale_branches:
        subject = f"✅ Branch Report - {org_name} - All branches are up to date"

    send_email(
        subject=subject,
        html_content=html_report,
        recipient=recipient_email,
        smtp_server=smtp_server,
        smtp_port=smtp_port,
        sender_email=sender_email,
        sender_password=sender_password
    )

    print("\n" + "=" * 60)
    print("✅ Branch monitoring completed successfully!")
    print("=" * 60)


if __name__ == "__main__":
    main()
