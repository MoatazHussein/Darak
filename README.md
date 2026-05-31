# Darak

Darak is an ASP.NET Core backend API for a contractor marketplace. Clients can register, create project requests, and manage request status. Contractors can register, browse requests, and submit proposals. The API also supports service categories, image upload, OTP flows, email flows, JWT authentication, and payment gateway configuration.

The solution targets **.NET 8** and follows **Clean Architecture** with **CQRS** through MediatR.

## Contents

- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database](#database)
- [API Overview](#api-overview)
- [Authentication and Roles](#authentication-and-roles)
- [Logging](#logging)

## Features

- Client and contractor registration
- Password login and SMS OTP login
- Registration OTP verification
- Email confirmation and password reset
- JWT bearer authentication
- Role management for Admin, Client, and Contractor users
- Service category management
- Project request CRUD and status updates
- Project proposal CRUD and status updates
- Local image upload and static file serving
- SQL Server persistence with Entity Framework Core
- Startup seeders for application bootstrap data
- Tap payment gateway configuration
- Serilog console and rolling file logs

## Architecture

The solution uses four projects with dependencies flowing inward:

```text
Darak.API
  -> Darak.Infrastructure
  -> Darak.Application
  -> Darak.Domain
```

Project responsibilities:

| Project | Responsibility |
| --- | --- |
| `Darak.API` | Controllers, middleware, dependency injection setup, Swagger, static files, application startup |
| `Darak.Application` | CQRS commands and queries, handlers, DTOs, validators, mapping profiles, application interfaces |
| `Darak.Infrastructure` | EF Core DbContext, repositories, migrations, seeders, email, SMS, security, storage, unit of work |
| `Darak.Domain` | Entities, enums, constants, and domain exceptions |

Typical request flow:

```text
HTTP request
  -> API controller
  -> MediatR command/query
  -> Application handler
  -> Infrastructure implementation
  -> Database or external service
```

## Tech Stack

| Concern | Technology |
| --- | --- |
| Runtime | .NET 8 |
| Web framework | ASP.NET Core Web API |
| Architecture | Clean Architecture, CQRS |
| Mediator | MediatR |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Identity | ASP.NET Core Identity |
| Authentication | JWT Bearer |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Query helpers | LinqKit |
| API docs | Swagger / Swashbuckle |
| Logging | Serilog |
| SMS | Infobip |
| Email | SMTP |
| Payments | Tap |

## Project Structure

```text
Darak/
|-- Darak.sln
|-- README.md
|-- src/
    |-- Darak.API/
    |   |-- Controllers/
    |   |-- Extensions/
    |   |-- Middlewares/
    |   |-- Properties/
    |   |-- Program.cs
    |   |-- appsettings.json
    |   `-- appsettings.Development.json
    |-- Darak.Application/
    |   |-- Common/
    |   |-- Extensions/
    |   `-- Features/
    |-- Darak.Infrastructure/
    |   |-- Extensions/
    |   |-- Migrations/
    |   |-- Persistence/
    |   |-- Repositories/
    |   |-- Seeders/
    |   |-- Services/
    |   `-- Startup/
    `-- Darak.Domain/
        |-- Constants/
        |-- Entities/
        |-- Enums/
        `-- Exceptions/
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server, SQL Server Express, or a reachable SQL Server instance
- EF Core CLI:

```bash
dotnet tool install --global dotnet-ef
```

### Restore and Build

```bash
dotnet restore Darak.sln
dotnet build Darak.sln
```

### Configure Local Settings

For local development, update `src/Darak.API/appsettings.Development.json` or use user secrets / environment variables for sensitive values.

Recommended user secrets setup:

```bash
dotnet user-secrets init --project src/Darak.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\\SQLExpress;Database=DarakDb;Trusted_Connection=True;TrustServerCertificate=True" --project src/Darak.API
dotnet user-secrets set "Jwt:Key" "<strong-jwt-signing-key>" --project src/Darak.API
dotnet user-secrets set "Jwt:Issuer" "https://localhost:7072" --project src/Darak.API
dotnet user-secrets set "Jwt:Audience" "https://localhost:7072" --project src/Darak.API
```

### Apply Migrations

```bash
dotnet ef database update --project src/Darak.Infrastructure --startup-project src/Darak.API
```

### Run the API

```bash
dotnet run --project src/Darak.API
```

Launch profiles expose:

- HTTPS: `https://localhost:7072`
- HTTP: `http://localhost:5241`
- Swagger: `/swagger`
- Health/root endpoint: `/`

## Configuration

Important configuration sections:

| Section | Purpose |
| --- | --- |
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `App:BaseUrl` | Public API base URL |
| `App:FrontendBaseUrl` | Frontend URL used for redirect links |
| `App:DefaultTimeZone` | Default time zone for date/time conversions |
| `Jwt` | Token signing key, issuer, and audience |
| `Tap` | Payment gateway API settings |
| `Smtp` | Email host, port, username, and password |
| `Infobip` | SMS gateway base URL, API key, and sender |
| `Otp` | OTP code length, expiration, and max attempts |
| `Serilog` | Logging levels and sinks |

Do not commit production secrets. Keep connection strings, JWT signing keys, SMTP passwords, SMS API keys, and payment keys in user secrets, environment variables, or a deployment secret store.

Example shape:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<server>;Database=<database>;User Id=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  },
  "App": {
    "BaseUrl": "https://localhost:7072",
    "FrontendBaseUrl": "https://localhost:3000",
    "DefaultTimeZone": "Arab Standard Time"
  },
  "Jwt": {
    "Key": "<strong-jwt-signing-key>",
    "Issuer": "https://localhost:7072",
    "Audience": "https://localhost:7072"
  },
  "Tap": {
    "SecretKey": "<tap-secret-key>",
    "ApiBase": "https://api.tap.company/",
    "PublicBaseUrl": "https://localhost:7072"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "<smtp-username>",
    "Password": "<smtp-password>"
  },
  "Infobip": {
    "BaseUrl": "<infobip-base-url>",
    "ApiKey": "<infobip-api-key>",
    "From": "Darak App"
  },
  "Otp": {
    "Length": 6,
    "TtlSeconds": 300,
    "MaxAttempts": 5
  }
}
```

## Database

Migrations are stored in `src/Darak.Infrastructure/Migrations`.

Add a new migration:

```bash
dotnet ef migrations add <MigrationName> --project src/Darak.Infrastructure --startup-project src/Darak.API
```

Apply migrations:

```bash
dotnet ef database update --project src/Darak.Infrastructure --startup-project src/Darak.API
```

The application runs registered seeders and startup tasks on startup.

## API Overview

Base route: `/api`

### Identity

Route prefix: `/api/identity`

| Method | Route | Auth | Description |
| --- | --- | --- | --- |
| `POST` | `/register/client` | Anonymous | Register a client |
| `POST` | `/register/contractor` | Anonymous | Register a contractor |
| `POST` | `/request-registration-otp` | Anonymous | Send registration OTP |
| `POST` | `/verify-client-registration-otp` | Anonymous | Verify client registration OTP |
| `POST` | `/login` | Anonymous | Password login |
| `POST` | `/request-login-otp` | Anonymous | Send login OTP |
| `POST` | `/verify-login-otp` | Anonymous | Verify login OTP |
| `POST` | `/forgot-password` | Anonymous | Request password reset link |
| `POST` | `/reset-password` | Anonymous | Reset password |
| `GET` | `/reset-password` | Anonymous | Redirect to frontend reset password page |
| `GET` | `/confirm-email` | Anonymous | Confirm email and redirect |
| `PATCH` | `/update/client` | Anonymous | Update client profile |
| `PATCH` | `/update/contractor` | Anonymous | Update contractor profile |
| `GET` | `/users` | Anonymous | List users |
| `GET` | `/users/me` | Authenticated | Get current user |
| `POST` | `/userRole` | Admin | Assign user role |
| `DELETE` | `/userRole` | Admin | Remove user role |

### Project Requests

Route prefix: `/api/project-requests`

All endpoints require authentication.

| Method | Route | Role | Description |
| --- | --- | --- | --- |
| `GET` | `/` | Any authenticated user | List project requests |
| `GET` | `/{id}` | Any authenticated user | Get project request by ID |
| `POST` | `/` | Client | Create project request |
| `PATCH` | `/` | Client | Update project request |
| `PATCH` | `/status` | Client | Update project request status |
| `DELETE` | `/{id}` | Client | Delete project request |

### Project Proposals

Route prefix: `/api/project-proposals`

All endpoints require authentication.

| Method | Route | Role | Description |
| --- | --- | --- | --- |
| `GET` | `/` | Any authenticated user | List project proposals |
| `POST` | `/` | Contractor | Create project proposal |
| `PATCH` | `/` | Contractor | Update project proposal |
| `PATCH` | `/status` | Any authenticated user | Update proposal status |
| `DELETE` | `/{id}` | Contractor | Delete project proposal |

### Service Categories

Route prefix: `/api/serviceCategories`

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/{id}` | Get category by ID |
| `GET` | `/GetAllMatching` | List and filter categories |
| `POST` | `/` | Create category |
| `PATCH` | `/` | Update category |
| `DELETE` | `/{id}` | Delete category |

### Images

Route prefix: `/api/UploadImage`

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/image` | Upload an image using multipart form data |

Uploaded files are served from `/Storage`.

## Authentication and Roles

JWT bearer tokens are returned by the login endpoints. Send the token with authenticated requests:

```http
Authorization: Bearer <token>
```

Supported roles are defined in `Darak.Domain.Constants.UserRoles`:

- `Admin`
- `Client`
- `Contractor`

Users also have a `UserType` value. Contractors are associated with a service category.

## Logging

Serilog is configured in `src/Darak.API/appsettings*.json`.

Default sinks:

- Console output
- Rolling log files under `src/Darak.API/Logs`

Request logging is enabled through `UseSerilogRequestLogging()`, and `ErrorHandlingMiddleware` maps application exceptions to HTTP responses.

## Useful Commands

```bash
dotnet restore Darak.sln
dotnet build Darak.sln
dotnet run --project src/Darak.API
dotnet ef database update --project src/Darak.Infrastructure --startup-project src/Darak.API
```
