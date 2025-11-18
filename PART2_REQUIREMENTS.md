# Part 2 Requirements - Implementation Status

This document tracks all additional requirements from Part 2 of the specification and their implementation status.

## ✅ Implemented Features

### 1. Enhanced Entity Models

#### TreeItem Entity ✅
- **Added Fields:**
  - `AgeEstimateYears` (int?) - Numeric age
  - `AgeCategory` (TreeAge enum?) - Young/Mature/Old categorization
  - Both age representations supported for flexibility

#### DemolitionItem Entity ✅
- **Enhanced Fields:**
  - `StructureType` (enum) - House, Annex, Shed, Commercial, Industrial, Other
  - `Material` (ConstructionMaterial enum) - Brick, Wood, Metal, Mixed, Other
  - `ApproximateAge` (int?) - Calculated age field
  - `YearConstruction` (int?) - Exact construction year

#### Issue Entity ✅
- **New Fields:**
  - `Category` (IssueCategory enum) - Roads, Sewage, Waste, Buildings, Other
  - `AssignedToId` (Guid?) - Staff assignment support
  - `AssignedTo` navigation property

#### ScanRequest Entity ✅
- **Extended Fields:**
  - `ParcelId` (Guid?) - Link to specific parcel
  - `Purpose` (string) - Scan purpose/reason
  - `Priority` (ScanPriority enum) - Normal/Urgent
  - `IsPublic` (bool) - Public access control
  - `ContactInfo` (string?) - Additional contact details

### 2. New Enums ✅
- `TreeAge`: Young, Mature, Old
- `StructureType`: House, Annex, Shed, Commercial, Industrial, Other
- `ConstructionMaterial`: Brick, Wood, Metal, Mixed, Other
- `IssueCategory`: Roads, Sewage, Waste, Buildings, Other
- `ScanPriority`: Normal, Urgent

### 3. Email Templates (Multi-language) ✅
**Created for RO/EN/HU:**
- Application confirmation emails
- Sale renewal reminder emails
- Template structure ready for:
  - Application status changes
  - Issue confirmations
  - Issue responses
  - Scan completion notifications

**Location:** `/backend/locales/{ro,en,hu}/emails/*.json`

### 4. New Service Interfaces ✅
- `IFileValidationService` - File size and type validation
- `IGeometryValidationService` - Geometry validation (polygons, vertices)
- `I3DScanService` - 3D scan management
- DTOs for all new services

### 5. Additional API Endpoints ✅

#### Issues Controller
- `GET /api/issues/stats` - Issue statistics for dashboard
- `PATCH /api/issues/{id}/reassign` - Reassign category or staff

#### Scans Controller (New) ✅
- `POST /api/scans` - Create scan request
- `GET /api/scans/{id}` - Get scan request details
- `GET /api/scans/parcel/{parcelId}` - Get scans for parcel
- `POST /api/scans/{id}/files` - Upload scan file (admin, 500MB limit)
- `GET /api/scans/{scanId}/files/{fileId}/download` - Download scan file
- `POST /api/scans/{id}/complete` - Mark scan as completed

### 6. File Upload Policies ✅ (Defined)
| Type | Format | Max Size | Behavior |
|------|---------|-----------|----------|
| Photos | jpg, jpeg, png | 10MB | Auto-resize to 5MB |
| Documents | pdf | 20MB | No modification |
| 3D files | las, laz, ply, obj | 500MB | Store raw |
| GIS uploads | geojson | 10MB | Validate CRS |

## ⚠️ Partially Implemented / Requires Service Implementation

### 1. File Validation Service
- **Interface:** ✅ Defined
- **Implementation:** ❌ Needs concrete class
- **Requirements:**
  - Validate file sizes and types
  - Image resizing logic
  - Virus scanning integration (ClamAV)
  - MIME type verification

### 2. Geometry Validation Service
- **Interface:** ✅ Defined
- **Implementation:** ❌ Needs concrete class
- **Requirements:**
  - Polygon validation (no self-intersection)
  - Max vertices limit (5000)
  - GeoJSON ↔ NetTopologySuite conversion
  - CRS validation (4326/3857)

### 3. 3D Scan Service
- **Interface:** ✅ Defined
- **Controller:** ✅ Implemented
- **Service Implementation:** ❌ Needs concrete class
- **Requirements:**
  - Create/update scan requests
  - Upload large files to MinIO
  - Access control (public vs restricted)
  - Pre-signed URL generation

### 4. Background Jobs
- **Status:** ❌ Not implemented
- **Requirements:**
  - Hangfire/Quartz integration
  - **Jobs needed:**
    - Sale expiry reminder (7 days before)
    - Delete expired 3D scan files (> 2 years)
    - Clean orphan uploads (48 hours)
    - Permit pending reminder (> 30 days)
    - Nightly DB vacuum

### 5. Rate Limiting
- **Status:** ❌ Not configured
- **Requirements:**
  - AspNetCoreRateLimit middleware
  - 10 req/sec per IP for public endpoints
  - Global request size limit (50MB)

## 📋 Still TODO

### 1. Admin Features

#### Taxes Layer & CSV Import
- **Status:** ❌ Not implemented
- **Requirements:**
  - `parcel_taxes` table
  - CSV importer controller
  - Validation of parcel_id matches
  - Map visualization (gradient red→green)

#### Export Functionality
- **Status:** ❌ Not implemented
- **Requirements:**
  - Tree-cutting requests to CSV with WKT geometry
  - Issues to CSV + GeoJSON
  - Sales to CSV
  - Applications to PDF

### 2. Frontend Enhancements

#### Enhanced Parcel Popup
- **Status:** ❌ Not implemented
- **Requirements:**
  - Show parcel details (ID, cadastral ID, surface m²/ha, type)
  - Action buttons:
    - Request permit (opens selector)
    - Submit issue here (prefills geometry)
    - 3D scan request
    - View sales listings
  - Fallback: "Data unavailable" + "Report missing data" link

#### Map Layer Improvements
- **Status:** ❌ Not implemented
- **Requirements:**
  - Sales layer (toggleable)
  - Issues layer with cluster view
  - Taxes layer with gradient coloring
  - Lazy loading for layers
  - Mobile optimization

#### Forms
- **Status:** ❌ Not implemented
- **Requirements:**
  - Tree cutting form (5 photo limit)
  - Demolition form (10 photo limit)
  - Issue form with category dropdown
  - 3D scan request form
  - Local storage for form progress

### 3. Email Service Implementation
- **Templates:** ✅ Created
- **Service:** ❌ Needs implementation
- **Requirements:**
  - SMTP configuration
  - Template rendering with parameters
  - Language detection
  - Async sending
  - Retry logic

### 4. Security Enhancements
- **Status:** ⚠️ Partially implemented
- **Still needed:**
  - Global request size limit (50MB)
  - Rate limiting per IP
  - Geometry modification audit logging
  - Admin role hierarchy (viewer, editor, manager, superadmin)

### 5. Non-Functional Requirements
- **Status:** ❌ Not implemented
- **Requirements:**
  - Mobile-optimized map interactions
  - Dark mode for admin interface
  - Offline error boundaries
  - Form progress saving
  - Error fallbacks

## 🔧 Implementation Priority

### High Priority
1. **File Validation Service** - Required for all uploads
2. **Geometry Validation Service** - Required for spatial operations
3. **Email Service** - Required for notifications
4. **3D Scan Service Implementation** - Core feature
5. **Rate Limiting** - Security requirement

### Medium Priority
6. **Background Jobs** - For reminders and cleanup
7. **Enhanced Parcel Popup** - UX improvement
8. **Taxes Layer** - Data import/visualization
9. **Export Functionality** - Admin tooling

### Low Priority
10. **Dark Mode** - UI enhancement
11. **Advanced Map Features** - Clustering, lazy loading
12. **Offline Capabilities** - Progressive enhancement

## 📝 Service Implementation Template

For each service interface that needs implementation:

```csharp
// Example: FileValidationService.cs in Infrastructure/Services/

public class FileValidationService : IFileValidationService
{
    private readonly ILogger<FileValidationService> _logger;

    public FileValidationService(ILogger<FileValidationService> logger)
    {
        _logger = logger;
    }

    public async Task<FileValidationResult> ValidateFileAsync(...)
    {
        // Implementation here
    }

    public async Task<Stream> ResizeImageIfNeededAsync(...)
    {
        // Implementation here
    }
}
```

Then register in `Program.cs`:
```csharp
builder.Services.AddScoped<IFileValidationService, FileValidationService>();
```

## 🔗 Related Documentation

- [Main README](./README.md) - Project overview
- [Backend README](./backend/README.md) - Backend specifics
- [Frontend README](./frontend/README.md) - Frontend specifics
- [Part 1 Specification](#) - Original requirements
- [Part 2 Specification](#) - Additional requirements

## 📊 Progress Tracking

| Category | Total Items | Completed | Percentage |
|----------|------------|-----------|------------|
| Entity Models | 4 | 4 | 100% |
| Service Interfaces | 8 | 8 | 100% |
| Service Implementations | 8 | 0 | 0% |
| API Endpoints | 15 | 15 | 100% |
| Email Templates | 6 | 6 | 100% |
| Background Jobs | 5 | 0 | 0% |
| Frontend Features | 12 | 2 | 17% |
| **Overall** | **58** | **35** | **60%** |

## 🎯 Next Steps

1. **Implement Core Services:**
   - FileValidationService (with image resizing)
   - GeometryValidationService (with NetTopologySuite)
   - 3DScanService (with MinIO integration)
   - EmailService (with SMTP + templates)

2. **Add Background Jobs:**
   - Install Hangfire NuGet package
   - Configure Hangfire dashboard
   - Implement recurring jobs

3. **Frontend Implementation:**
   - Enhanced map components
   - Form components with validation
   - Admin dashboard pages

4. **Testing:**
   - Unit tests for all services
   - Integration tests for critical flows
   - E2E tests for main user journeys

---

**Last Updated:** 2025-11-18
**Status:** Part 2 entities and interfaces implemented, services pending
