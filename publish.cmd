@echo off
setlocal
cd /d "%~dp0"

where gh >nul 2>&1
if errorlevel 1 (
    echo GitHub CLI is required. Install it from https://cli.github.com/
    exit /b 1
)

gh auth status >nul 2>&1
if errorlevel 1 (
    echo Sign in first using: gh auth login
    exit /b 1
)

git remote get-url origin >nul 2>&1
if not errorlevel 1 (
    echo Remote origin already exists. Check it before publishing:
    git remote -v
    exit /b 1
)

gh repo create otus-prototype --private --description "OTUS C#: Prototype pattern with typed deep cloning" --source=. --remote=origin --push
if errorlevel 1 exit /b 1

gh repo view --json url --jq .url
exit /b %errorlevel%
