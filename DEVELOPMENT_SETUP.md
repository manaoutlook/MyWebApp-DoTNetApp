# Development Environment Setup Guide

This guide provides instructions for setting up the development environment on both Windows and macOS.

## Prerequisites

### Both Platforms
- Git
- .NET SDK (version 8.0 or higher)
- Visual Studio Code or Visual Studio
- SQLite (comes with .NET SDK)

### Windows Specific
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (Community edition or higher)
- [Git for Windows](https://git-scm.com/download/win)

### macOS Specific
- [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Homebrew](https://brew.sh/) (recommended for package management)

## Setup Steps

### 1. Clone the Repository
```bash
git clone https://github.com/your-organization/MyWebApp.git
cd MyWebApp
```

### 2. Install .NET SDK
#### Windows
- Download and install from [.NET download page](https://dotnet.microsoft.com/download)
- Verify installation:
  ```cmd
  dotnet --version
  ```

#### macOS
- Using Homebrew:
  ```bash
  brew install --cask dotnet-sdk
  ```
- Verify installation:
  ```bash
  dotnet --version
  ```

### 3. Install IDE
#### Windows
- Install Visual Studio 2022 with ASP.NET and web development workload

#### macOS
- Install Visual Studio for Mac or Visual Studio Code with C# extension

### 4. Database Setup
The application uses SQLite which is included with .NET SDK. No additional setup is required.

### 5. Build and Run
```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### 6. Verify Setup
- Open browser and navigate to https://localhost:5001
- You should see the application running

## Troubleshooting

### Common Issues
1. **Port conflicts**: If port 5001 is in use, update `Properties/launchSettings.json`
2. **Database issues**: Delete `MyWebApp.db` and restart the application
3. **Missing dependencies**: Run `dotnet restore` and rebuild

## Additional Resources
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
