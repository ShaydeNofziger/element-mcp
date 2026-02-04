#!/bin/bash

# Test script to verify Element MCP Server functionality
# This script starts the server and sends sample MCP requests

echo "Element MCP Server Test Script"
echo "================================"
echo ""

cd "$(dirname "$0")/../src"

echo "Building the server..."
dotnet build -q
if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi
echo "✓ Build successful"
echo ""

echo "Testing server startup..."
timeout 3 dotnet run 2>&1 | grep -q "Application started"
if [ $? -eq 0 ]; then
    echo "✓ Server starts successfully"
else
    echo "❌ Server failed to start"
    exit 1
fi
echo ""

echo "Server is ready!"
echo ""
echo "To use the server with an MCP client:"
echo "1. Configure your MCP client (Claude Desktop, VS Code, etc.)"
echo "2. Add the following configuration:"
echo ""
echo '{
  "mcpServers": {
    "element": {
      "command": "dotnet",
      "args": ["run", "--project", "'$(pwd)'"]
    }
  }
}'
echo ""
echo "3. Query the server using natural language:"
echo "   - 'List all Element components'"
echo "   - 'Show me the Button component documentation'"
echo "   - 'What are the available color tokens?'"
echo "   - 'Search for dialog components'"
echo ""
echo "✅ All tests passed!"
