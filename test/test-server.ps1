#!/usr/bin/env pwsh
# Test script to verify Element MCP Server functionality
# This script is cross-platform and works on Windows, macOS, and Linux

Write-Host "Element MCP Server Test Script" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Change to the src directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcDir = Join-Path $scriptDir ".." "src"
Set-Location $srcDir

Write-Host "Building the server..."
$buildOutput = dotnet build -q 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed" -ForegroundColor Red
    Write-Host $buildOutput
    exit 1
}
Write-Host "✓ Build successful" -ForegroundColor Green
Write-Host ""

Write-Host "Testing server startup..."

# Create a temporary file for server output
$logFile = New-TemporaryFile

# Start server in background
$serverJob = Start-Job -ScriptBlock {
    param($srcPath)
    Set-Location $srcPath
    dotnet run 2>&1
} -ArgumentList (Get-Location).Path

# Wait up to 5 seconds for server to start
$maxAttempts = 50
$found = $false

for ($i = 0; $i -lt $maxAttempts; $i++) {
    $output = Receive-Job -Job $serverJob 2>&1
    if ($output) {
        $output | Out-File -FilePath $logFile -Append
        if ($output -match "Application started") {
            Write-Host "✓ Server starts successfully" -ForegroundColor Green
            $found = $true
            break
        }
    }
    Start-Sleep -Milliseconds 100
}

# Clean up the server job
Stop-Job -Job $serverJob -ErrorAction SilentlyContinue
Remove-Job -Job $serverJob -ErrorAction SilentlyContinue

# Check if we found the message
if (-not $found) {
    Write-Host "❌ Server failed to start" -ForegroundColor Red
    Write-Host "Server output:"
    Get-Content $logFile
    Remove-Item $logFile -ErrorAction SilentlyContinue
    exit 1
}

Remove-Item $logFile -ErrorAction SilentlyContinue
Write-Host ""

Write-Host "Server is ready!" -ForegroundColor Green
Write-Host ""
Write-Host "To use the server with an MCP client:"
Write-Host "1. Configure your MCP client (Claude Desktop, VS Code, etc.)"
Write-Host "2. Add the following configuration:"
Write-Host ""

$currentPath = (Get-Location).Path
$config = @"
{
  "mcpServers": {
    "element": {
      "command": "dotnet",
      "args": ["run", "--project", "$currentPath"]
    }
  }
}
"@
Write-Host $config

Write-Host ""
Write-Host "3. Query the server using natural language:"
Write-Host "   - 'List all Element components'"
Write-Host "   - 'Show me the Button component documentation'"
Write-Host "   - 'What are the available color tokens?'"
Write-Host "   - 'Search for dialog components'"
Write-Host ""
Write-Host "✅ All tests passed!" -ForegroundColor Green
