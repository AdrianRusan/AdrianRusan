# PUG Platform Backend

ASP.NET Core 8.0 Web API for the Local Council PUG Platform MVP.

## Architecture

- **Domain**: Entities, Value Objects, Enums
- **Application**: DTOs, Interfaces, Services, Validation
- **Infrastructure**: DbContext, Repositories, External Services (MinIO, Email)
- **Api**: Controllers, Middleware, Configuration

## Tech Stack

- .NET 8.0
- Entity Framework Core with Npgsql
- NetTopologySuite for PostGIS/geometry support
- MinIO for object storage
- JWT authentication with refresh tokens
- Swagger/OpenAPI documentation

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL 15+ with PostGIS extension
- MinIO or S3-compatible storage

## Configuration

Configure the following in `appsettings.json` or environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=pugplatform;Username=postgres;Password=postgres"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "PugPlatform",
    "Audience": "PugPlatformClient"
  },
  "MinIO": {
    "Endpoint": "localhost:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin"
  }
}
```

## Running Locally

```bash
# Restore dependencies
dotnet restore

# Run migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Api

# Run the API
dotnet run --project src/Api
```

The API will be available at `https://localhost:5001` and Swagger UI at `https://localhost:5001/swagger`.

## Running with Docker

```bash
docker build -t pug-platform-backend .
docker run -p 8080:8080 pug-platform-backend
```

## API Endpoints

### Authentication
- POST `/api/auth/register` - Register new user
- POST `/api/auth/login` - Login
- POST `/api/auth/refresh` - Refresh token
- POST `/api/auth/logout` - Logout

### Parcels
- GET `/api/parcels/{id}` - Get parcel by ID
- GET `/api/parcels` - Get parcels by bounding box

### Applications
- POST `/api/applications` - Create application
- GET `/api/applications/{id}` - Get application
- GET `/api/applications` - List applications
- POST `/api/applications/{id}/transition` - Change status (staff only)
- POST `/api/applications/{id}/attachments` - Upload attachment

### Issues
- POST `/api/issues` - Create issue (public)
- GET `/api/issues/{id}` - Get issue
- GET `/api/issues` - List issues (staff only)
- POST `/api/issues/{id}/respond` - Respond to issue (staff only)
- POST `/api/issues/{id}/photos` - Upload photo

### Sales
- POST `/api/sales` - Create sale listing
- GET `/api/sales/{id}` - Get sale
- GET `/api/sales` - List active sales
- POST `/api/sales/{id}/renew` - Renew listing
- POST `/api/sales/{id}/photos` - Upload photo

## Development Notes

### Geometry Handling

All geometry data is stored in SRID 4326 (WGS84). For metric calculations, the system automatically transforms to SRID 3857 (Web Mercator).

The API accepts and returns GeoJSON Feature/FeatureCollection objects:

```json
{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [25.6012, 45.6532]
  },
  "properties": {}
}
```

### File Upload Limits

- Images: JPEG/PNG, max 10MB
- Documents: PDF, max 20MB
- 3D files: LAZ/LAS, configurable limit

### Authentication & Authorization

The API uses JWT with refresh tokens. Access tokens expire after 60 minutes, refresh tokens after 7 days.

Roles:
- PublicUser: Anonymous access
- RegisteredUser: Authenticated user
- UrbanismStaff: Staff member
- Admin: Administrator
- SuperAdmin: Super administrator

## Testing

```bash
# Run unit tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## Database Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/Api

# Update database
dotnet ef database update --project src/Infrastructure --startup-project src/Api

# Generate SQL script
dotnet ef migrations script --project src/Infrastructure --startup-project src/Api
```

## TODO for Production

- [ ] Implement service layer implementations (currently interfaces only)
- [ ] Add FluentValidation validators for all DTOs
- [ ] Implement email service with localized templates
- [ ] Add comprehensive unit and integration tests
- [ ] Implement rate limiting and request throttling
- [ ] Add health checks endpoint
- [ ] Configure proper logging and monitoring
- [ ] Add API versioning
- [ ] Implement caching strategy
- [ ] Add database connection resilience
- [ ] Configure HTTPS with proper certificates
- [ ] Implement audit logging for sensitive operations
- [ ] Add background jobs for cleanup and notifications
