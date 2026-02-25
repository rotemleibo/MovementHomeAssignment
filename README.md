# MovementHomeAssignment

A .NET 10 Web API with MySQL and Redis caching.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started) and Docker Compose

## Setup

### 1. Clone the repository

```bash
git clone https://github.com/rotemleibo/MovementHomeAssignment.git
cd MovementHomeAssignment
```

### 2. Start the dependencies (MySQL and Redis)

```bash
docker compose up -d
```

This starts:
- **MySQL 8.0** on port `3306` (database: `MovementHomeAssignmentDB`, user: `appuser`, password: `apppassword`)
- **Redis 7.4** on port `6379`

### 3. Run the API

```bash
dotnet run --project MovementHomeAssignment.API/MovementHomeAssignment.API.csproj
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).  
Swagger UI is available at `https://localhost:5001/swagger` when running in Development mode.

### 4. Configuration

Connection strings and cache settings are defined in `MovementHomeAssignment.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=MovementHomeAssignmentDB;Uid=appuser;Pwd=apppassword;",
    "Redis": "localhost:6379"
  },
  "RedisOptions": {
    "TTL": 6
  },
  "InMemoryCache": {
    "Capacity": 3
  }
}
```

These defaults match the Docker Compose configuration and require no changes for local development.

## Running Tests

```bash
dotnet test MovementHomeAssignment.Tests/MovementHomeAssignment.Tests.csproj
```

## API Endpoints

| Method | Route        | Description          |
|--------|--------------|----------------------|
| GET    | /User/{id}   | Get a user by ID     |
| POST   | /User        | Create a new user    |

## Stopping the dependencies

```bash
docker compose down
```