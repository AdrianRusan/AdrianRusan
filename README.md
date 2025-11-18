# PUG Platform - Local Council Urban Planning Management System

A comprehensive platform for managing urban planning applications, parcel data, issues reporting, and land sales for local councils in Romania.

## 🎯 Project Overview

The PUG Platform is an MVP (Minimum Viable Product) designed to digitize and streamline urban planning processes for local councils. It provides tools for citizens to submit applications, report issues, and browse land sales, while giving staff the ability to manage and process these requests efficiently.

### Key Features

- **📍 Interactive Map**: View parcels, applications, issues, and sales on an interactive PostGIS-powered map
- **📝 Application Management**: Submit and track tree cutting, demolition, and construction permit applications
- **🚧 Issue Reporting**: Public issue reporting with photo uploads
- **🏘️ Land Marketplace**: Browse and create land sale listings
- **👥 Role-Based Access**: Public, registered user, staff, and admin roles
- **🌍 Multi-language**: Romanian, English, and Hungarian support
- **📊 Admin Dashboard**: Comprehensive management interface for staff
- **📎 File Management**: Secure file storage with MinIO (S3-compatible)
- **🔐 Secure Authentication**: JWT with refresh tokens

## 🏗️ Architecture

### Stack

**Backend:**
- ASP.NET Core 8.0 Web API
- Entity Framework Core with PostgreSQL + PostGIS
- NetTopologySuite for geometry handling
- MinIO for object storage
- JWT authentication
- Swagger/OpenAPI documentation

**Frontend:**
- Next.js 14 (React) with App Router
- TypeScript
- TailwindCSS
- MapLibre GL for maps
- React Hook Form
- TanStack Query

**Infrastructure:**
- Docker & Docker Compose
- PostgreSQL 15 with PostGIS 3.4
- MinIO
- Optional Kubernetes/Helm charts

### Repository Structure

```
/
├── backend/              # ASP.NET Core Web API
│   ├── src/
│   │   ├── Api/         # Controllers, Program.cs
│   │   ├── Application/ # DTOs, Services, Interfaces
│   │   ├── Domain/      # Entities, Enums
│   │   └── Infrastructure/ # DbContext, Migrations, Storage
│   ├── Dockerfile
│   └── README.md
├── frontend/            # Next.js application
│   ├── app/            # Next.js pages
│   ├── components/     # React components
│   ├── services/       # API clients
│   ├── types/          # TypeScript definitions
│   ├── Dockerfile
│   └── README.md
├── infra/              # Infrastructure configuration
│   ├── docker-compose/ # Docker Compose setup
│   └── helm/           # Kubernetes manifests (optional)
├── docs/               # Documentation
└── README.md           # This file
```

## 🚀 Quick Start

### Prerequisites

- Docker 20.10+ and Docker Compose 2.0+
- OR: .NET 8 SDK + Node.js 20+ + PostgreSQL 15+ with PostGIS

### Running with Docker Compose (Recommended)

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd AdrianRusan
   ```

2. **Start all services:**
   ```bash
   cd infra/docker-compose
   docker compose up -d
   ```

3. **Access the applications:**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger
   - MinIO Console: http://localhost:9001 (minioadmin/minioadmin)

4. **Sample users** (password: `Password123!`):
   - Admin: admin@example.com
   - Staff: staff@example.com
   - User: user@example.com

### Running Locally (Development)

**Backend:**
```bash
cd backend
dotnet restore
dotnet ef database update --project src/Infrastructure --startup-project src/Api
dotnet run --project src/Api
```

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```

## 📊 Database Schema

The system uses PostgreSQL with PostGIS extension. Key tables:

- **users**: User accounts with roles
- **parcels**: Land parcels with geometry (SRID 4326)
- **applications**: Generic applications table
- **tree_items, demolition_items, construction_items**: Application-specific details
- **issues**: Public issue reports
- **sales**: Land sale listings
- **attachments**: File metadata
- **audit_logs**: System audit trail

All geometry columns use SRID 4326 (WGS84) for storage. Metric calculations transform to SRID 3857.

## 🔌 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - Logout

### Parcels
- `GET /api/parcels/{id}` - Get parcel by ID
- `GET /api/parcels?bbox=...` - Get parcels in bounding box

### Applications
- `POST /api/applications` - Create application (authenticated)
- `GET /api/applications/{id}` - Get application details
- `GET /api/applications` - List applications with filters
- `POST /api/applications/{id}/transition` - Change status (staff only)
- `POST /api/applications/{id}/attachments` - Upload file

### Issues
- `POST /api/issues` - Create issue (public)
- `GET /api/issues/{id}` - Get issue
- `GET /api/issues` - List issues (staff only)
- `POST /api/issues/{id}/respond` - Respond to issue (staff only)

### Sales
- `POST /api/sales` - Create sale listing (authenticated)
- `GET /api/sales` - List active sales
- `GET /api/sales/{id}` - Get sale details
- `POST /api/sales/{id}/renew` - Renew listing

See [Swagger documentation](http://localhost:5000/swagger) for full API reference.

## 🗺️ Spatial Data

### GeoJSON Format

All geometry is exchanged via GeoJSON Feature/FeatureCollection:

```json
{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [26.0962, 44.4379]
  },
  "properties": {
    "name": "Example Point"
  }
}
```

### Coordinate Systems

- **Storage**: SRID 4326 (WGS84 lat/lon)
- **Calculations**: SRID 3857 (Web Mercator) for metric measurements
- Surface area computed automatically via PostGIS triggers

## 🧪 Testing

### Backend Tests
```bash
cd backend
dotnet test
```

### Frontend Tests
```bash
cd frontend
npm run test
```

### E2E Tests
```bash
cd frontend
npm run test:e2e
```

## 📦 Deployment

### Docker Compose (Production)

1. Update configuration in `docker-compose.yml`
2. Set secure passwords and keys
3. Configure HTTPS with reverse proxy (nginx/Traefik)
4. Run with production profile

### Kubernetes

Helm charts are provided in `infra/helm/`:

```bash
helm install pug-platform ./infra/helm/pug-platform \
  --set postgresql.password=secure_password \
  --set minio.rootPassword=secure_password \
  --set jwt.secretKey=your_secret_key
```

## 🔐 Security

- JWT authentication with refresh tokens
- httpOnly cookies for refresh tokens
- Role-based authorization
- Input validation with FluentValidation
- SQL injection protection via EF Core
- XSS protection
- CSRF protection
- Rate limiting (planned)
- File upload size limits and type validation

## 🌍 Internationalization

The platform supports:
- **Romanian** (ro) - Default
- **English** (en)
- **Hungarian** (hu)

Translation files are in:
- Backend: `locales/{ro,en,hu}/emails/*.json`
- Frontend: `frontend/public/locales/{ro,en,hu}/*.json`

## 📝 License

[Your License Here]

## 👥 Contributing

Contributions are welcome! Please read CONTRIBUTING.md for details.

## 🐛 Known Issues & Limitations (MVP)

- Service layer implementations are interface definitions only (requires implementation)
- Email service is stubbed (requires SMTP configuration)
- No payment integration
- No qualified digital signature support
- Background jobs for reminders not fully implemented
- Limited test coverage
- No rate limiting middleware configured
- Parcel dataset may be incomplete

## 🚧 Roadmap

### Phase 2
- [ ] Complete service implementations
- [ ] Email notifications with templates
- [ ] Background jobs for reminders and cleanup
- [ ] Advanced search and filtering
- [ ] Export functionality (PDF, CSV)
- [ ] Analytics dashboard

### Phase 3
- [ ] Payment integration
- [ ] Digital signature support
- [ ] Mobile application
- [ ] GIS data import/export tools
- [ ] Advanced reporting

## 📚 Documentation

- [Backend README](./backend/README.md)
- [Frontend README](./frontend/README.md)
- [Docker Compose Setup](./infra/docker-compose/README.md)
- [API Documentation](http://localhost:5000/swagger) (when running)

## 🆘 Support

For issues and questions:
- Create a GitHub issue
- Check existing documentation
- Review API documentation in Swagger

## 🎉 Acknowledgments

Built with:
- ASP.NET Core
- Next.js
- PostgreSQL/PostGIS
- MapLibre GL
- MinIO

---

## 👨‍💻 About the Developer

**Adrian Rusan** - Full-stack software developer with a passion for problem-solving and web development.

- 🔭 Working on: Full-stack web development with modern frameworks
- 🌱 Learning: React.js, GraphQL, Docker
- 💬 Ask me about: Software development, frontend to backend architecture
- 📫 Contact: [LinkedIn](https://www.linkedin.com/in/adrianrusan/) | rusan.adrian.ionut@gmail.com

---

**Note**: This is an MVP. Some features are scaffolded with interfaces and require full implementation. See individual README files in backend/frontend directories for detailed TODO lists.
