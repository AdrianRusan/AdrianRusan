# Docker Compose Setup

This directory contains the Docker Compose configuration for running the entire PUG Platform stack locally.

## Services

- **postgres**: PostgreSQL 15 with PostGIS 3.4
- **minio**: MinIO object storage
- **backend**: ASP.NET Core API
- **frontend**: Next.js application

## Prerequisites

- Docker 20.10+
- Docker Compose 2.0+

## Quick Start

```bash
# From the repository root
cd infra/docker-compose

# Start all services
docker compose up -d

# View logs
docker compose logs -f

# Stop all services
docker compose down

# Stop and remove volumes (clean slate)
docker compose down -v
```

## Service Access

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **MinIO Console**: http://localhost:9001 (login: minioadmin/minioadmin)
- **PostgreSQL**: localhost:5432 (database: pugplatform, user: postgres, password: postgres)

## Sample Users

The seed data includes three sample users (password for all: `Password123!`):

1. **Super Admin**
   - Email: admin@example.com
   - Role: SuperAdmin
   - Access: Full system access

2. **Staff Member**
   - Email: staff@example.com
   - Role: UrbanismStaff
   - Access: Can manage applications and issues

3. **Regular User**
   - Email: user@example.com
   - Role: RegisteredUser
   - Access: Can create applications and sales

## Sample Data

The seed script creates:
- 3 users (admin, staff, regular user)
- 2 sample parcels with geometry in Bucharest area
- 1 sample tree cutting application
- 1 sample issue report
- 1 sample sale listing
- Tax status for one parcel

## Database Management

```bash
# Connect to PostgreSQL
docker exec -it pug-postgres psql -U postgres -d pugplatform

# View spatial data
docker exec -it pug-postgres psql -U postgres -d pugplatform -c "SELECT 'ParcelId', \"ParcelId\", ST_AsGeoJSON(\"Geom\") FROM \"Parcels\";"

# Reset database
docker compose down -v
docker compose up -d
```

## MinIO Setup

MinIO is used for storing file attachments (photos, documents, 3D scans). The bucket `pug-platform` is automatically created when files are first uploaded.

To access MinIO console:
1. Navigate to http://localhost:9001
2. Login with: minioadmin / minioadmin
3. Browse buckets and files

## Troubleshooting

### Backend fails to start

Check that PostgreSQL is healthy:
```bash
docker compose logs postgres
docker compose ps
```

Wait for PostGIS initialization to complete.

### Frontend can't connect to backend

Ensure backend is running and healthy:
```bash
curl http://localhost:5000/swagger
```

### Database initialization errors

If migrations fail, manually run the schema:
```bash
docker exec -i pug-postgres psql -U postgres -d pugplatform < ../../backend/src/Infrastructure/Data/Migrations/InitialSchema.sql
```

## Development Workflow

1. Start services: `docker compose up -d`
2. Make code changes in backend or frontend
3. Rebuild specific service:
   ```bash
   docker compose up -d --build backend
   # or
   docker compose up -d --build frontend
   ```
4. View logs: `docker compose logs -f backend` or `docker compose logs -f frontend`
5. Stop services: `docker compose down`

## Environment Variables

Override default configuration by creating `.env` file:

```env
POSTGRES_PASSWORD=your_secure_password
MINIO_ROOT_USER=your_minio_user
MINIO_ROOT_PASSWORD=your_minio_password
JWT_SECRET_KEY=your_jwt_secret
```

Then start with:
```bash
docker compose --env-file .env up -d
```

## Network

All services run on the `pug-network` bridge network, allowing inter-service communication using service names (e.g., `backend` can reach `postgres` at `postgres:5432`).

## Volumes

Persistent data is stored in Docker volumes:
- `postgres-data`: PostgreSQL database files
- `minio-data`: MinIO object storage files

To backup data:
```bash
docker run --rm -v pug-postgres-data:/data -v $(pwd):/backup alpine tar czf /backup/postgres-backup.tar.gz /data
```
