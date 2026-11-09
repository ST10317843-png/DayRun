# DayRun - Event Management Platform

A fully functional ASP.NET Core 8 MVC web application consuming a RESTful API. Built for managing road running, walking, and cycling events in South Africa.

## CI/CD Status
![.NET CI](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/ci.yml/badge.svg)
*(Note: Insert your actual green CI screenshot below)*
![Green Build Screenshot](docs/ci-screenshot.png)

## Prerequisites
- .NET 8.0 SDK
- SQL Server LocalDB
- Docker Desktop (for containerization)

## Setup Instructions
1. Clone the repository.
2. Open `DayRun.sln` in Visual Studio 2022.
3. Ensure the API project is running on `http://localhost:5010`.
4. Update the database: Open Package Manager Console, set Default Project to `DayRun`, and run `Update-Database`.
5. Ensure `DayRun.Web/appsettings.json` has `"BaseUrl": "http://localhost:5010/"`.
6. Press `F5` to run both projects.

## Docker Setup
To build and run the MVC application in Docker:Access the app at http://localhost:8080.
```bash
docker build -t dayrun-mvc -f DayRun.Web/Dockerfile .
docker run -d -p 8080:8080 --name dayrun-app dayrun-mvc
