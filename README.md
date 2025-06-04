# Web API Project Setup

This document explains how to run and debug your ASP.NET Core Web API project in VS Code.

## Prerequisites

Make sure you have the following extensions installed in VS Code:
- C# (ms-dotnettools.csharp) ✓ *Already installed*
- C# Dev Kit (ms-dotnettools.csdevkit) ✓ *Already installed*
- .NET Install Tool (ms-dotnettools.vscode-dotnet-runtime) ✓ *Already installed*

## Project Structure

```
src/
├── web-api.sln                 # Solution file
├── web-api.web/               # Main Web API project
│   ├── Program.cs             # Application entry point
│   ├── web-api.web.csproj     # Project file
│   ├── web-api.web.http       # HTTP test file
│   └── Properties/
│       └── launchSettings.json # Launch configuration
└── web-api.tests/             # Unit tests project
    ├── UnitTest1.cs
    └── web-api.tests.csproj
```

## Running the Application

### Option 1: Using VS Code Debug/Run
1. Open the project in VS Code
2. Press `F5` or go to **Run and Debug** panel (Ctrl+Shift+D)
3. Select one of the launch configurations:
   - **Launch Web API** - Basic launch
   - **Launch Web API (HTTP)** - HTTP only with browser launch
   - **Launch Web API (HTTPS)** - HTTPS with browser launch
   - **Attach to Web API** - Attach to running process

### Option 2: Using VS Code Tasks
1. Press `Ctrl+Shift+P` to open command palette
2. Type "Tasks: Run Task"
3. Select one of the available tasks:
   - **build** - Build the solution
   - **run web-api** - Run the web API project
   - **watch** - Run with hot reload (file watching)
   - **test** - Run unit tests
   - **publish** - Publish the application

### Option 3: Using Terminal
```powershell
# Navigate to the project directory
cd "c:\Temp\web-api\src\web-api.web"

# Run the application
dotnet run

# Or run with specific profile
dotnet run --launch-profile http
dotnet run --launch-profile https
```

## API Endpoints

The application currently provides:

- **GET /weatherforecast** - Returns weather forecast data
- **GET /openapi** - OpenAPI/Swagger documentation (Development only)

## Testing the API

### Using the HTTP file
1. Open `web-api.web.http` in VS Code
2. Click "Send Request" above any HTTP request
3. View responses in the output panel

### Using Browser
- HTTP: http://localhost:5113/weatherforecast
- HTTPS: https://localhost:7259/weatherforecast

### Using curl
```powershell
# HTTP
curl http://localhost:5113/weatherforecast

# HTTPS
curl https://localhost:7259/weatherforecast
```

## Debugging

### Setting Breakpoints
1. Open any `.cs` file (e.g., `Program.cs`)
2. Click in the gutter to the left of line numbers to set breakpoints
3. Press `F5` to start debugging
4. Make requests to your API to hit the breakpoints

### Debug Configurations Available
- **Launch Web API**: Standard debugging with internal console
- **Launch Web API (HTTP)**: Debug HTTP-only mode with browser launch
- **Launch Web API (HTTPS)**: Debug HTTPS mode with browser launch
- **Attach to Web API**: Attach debugger to running process

### Hot Reload
Use the "watch" task for development with automatic reloading:
```powershell
dotnet watch run --project "c:\Temp\web-api\src\web-api.web"
```

## Development URLs

- **HTTP**: http://localhost:5113
- **HTTPS**: https://localhost:7259
- **OpenAPI/Swagger** (Development): http://localhost:5113/openapi

## Building and Testing

### Build
```powershell
dotnet build "c:\Temp\web-api\src\web-api.sln"
```

### Run Tests
```powershell
dotnet test "c:\Temp\web-api\src\web-api.tests"
```

### Publish
```powershell
dotnet publish "c:\Temp\web-api\src\web-api.sln"
```

## VS Code Configuration Files

The following configuration files have been created in `.vscode/`:

- **tasks.json** - Build, run, test, and watch tasks
- **launch.json** - Debug configurations
- **settings.json** - Project-specific settings

## Troubleshooting

### Common Issues

1. **Port already in use**: Change ports in `Properties/launchSettings.json`
2. **HTTPS certificate issues**: Run `dotnet dev-certs https --trust`
3. **Build errors**: Ensure .NET 9.0 SDK is installed
4. **Extension issues**: Restart VS Code or reload window

### Useful Commands
```powershell
# Check .NET version
dotnet --version

# Restore packages
dotnet restore

# Clean build
dotnet clean && dotnet build

# Trust HTTPS certificate
dotnet dev-certs https --trust
```
