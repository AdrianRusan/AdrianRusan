-- Enable PostGIS extension
CREATE EXTENSION IF NOT EXISTS postgis;

-- Users table
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Phone" VARCHAR(20),
    "Role" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "RefreshToken" TEXT,
    "RefreshTokenExpiryTime" TIMESTAMP
);

CREATE INDEX "IX_Users_Email" ON "Users"("Email");

-- Parcels table
CREATE TABLE IF NOT EXISTS "Parcels" (
    "Id" UUID PRIMARY KEY,
    "ParcelId" VARCHAR(50) NOT NULL,
    "OwnerId" UUID,
    "Geom" geometry(Polygon, 4326),
    "SurfaceM2" NUMERIC(18, 2) NOT NULL DEFAULT 0,
    "SurfaceHa" NUMERIC(18, 4) NOT NULL DEFAULT 0,
    "PlotType" VARCHAR(50) NOT NULL,
    "Properties" JSONB,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY ("OwnerId") REFERENCES "Users"("Id") ON DELETE SET NULL
);

CREATE INDEX "IX_Parcels_ParcelId" ON "Parcels"("ParcelId");
CREATE INDEX "IX_Parcels_Geom" ON "Parcels" USING GIST("Geom");

-- Applications table
CREATE TABLE IF NOT EXISTS "Applications" (
    "Id" UUID PRIMARY KEY,
    "ApplicationType" VARCHAR(50) NOT NULL,
    "ApplicantId" UUID NOT NULL,
    "Geom" geometry(Geometry, 4326),
    "Status" VARCHAR(50) NOT NULL,
    "TrackingNumber" VARCHAR(50) NOT NULL UNIQUE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "Data" JSONB,
    FOREIGN KEY ("ApplicantId") REFERENCES "Users"("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_Applications_TrackingNumber" ON "Applications"("TrackingNumber");
CREATE INDEX "IX_Applications_Geom" ON "Applications" USING GIST("Geom");

-- TreeItems table
CREATE TABLE IF NOT EXISTS "TreeItems" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "Geom" geometry(Point, 4326),
    "Species" VARCHAR(100) NOT NULL,
    "AgeEstimate" INTEGER,
    "DiameterCm" NUMERIC(10, 2),
    "Condition" TEXT,
    "Motivation" TEXT NOT NULL,
    FOREIGN KEY ("ApplicationId") REFERENCES "Applications"("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_TreeItems_Geom" ON "TreeItems" USING GIST("Geom");

-- DemolitionItems table
CREATE TABLE IF NOT EXISTS "DemolitionItems" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "Geom" geometry(Polygon, 4326),
    "StructureType" VARCHAR(100) NOT NULL,
    "YearConstruction" INTEGER,
    "Material" VARCHAR(100),
    "Motivation" TEXT NOT NULL,
    FOREIGN KEY ("ApplicationId") REFERENCES "Applications"("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_DemolitionItems_Geom" ON "DemolitionItems" USING GIST("Geom");

-- ConstructionItems table
CREATE TABLE IF NOT EXISTS "ConstructionItems" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "Geom" geometry(Polygon, 4326),
    "BuildingType" VARCHAR(100) NOT NULL,
    "PlannedArea" NUMERIC(10, 2),
    "Floors" INTEGER,
    "Description" TEXT,
    FOREIGN KEY ("ApplicationId") REFERENCES "Applications"("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ConstructionItems_Geom" ON "ConstructionItems" USING GIST("Geom");

-- Attachments table
CREATE TABLE IF NOT EXISTS "Attachments" (
    "Id" UUID PRIMARY KEY,
    "OwnerType" VARCHAR(50) NOT NULL,
    "OwnerId" UUID NOT NULL,
    "FileName" VARCHAR(255) NOT NULL,
    "ContentType" VARCHAR(100) NOT NULL,
    "Size" BIGINT NOT NULL,
    "StoragePath" VARCHAR(500) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX "IX_Attachments_OwnerType_OwnerId" ON "Attachments"("OwnerType", "OwnerId");

-- ApplicationComments table
CREATE TABLE IF NOT EXISTS "ApplicationComments" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "AuthorId" UUID NOT NULL,
    "Comment" TEXT NOT NULL,
    "IsInternal" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY ("ApplicationId") REFERENCES "Applications"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("AuthorId") REFERENCES "Users"("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_ApplicationComments_ApplicationId" ON "ApplicationComments"("ApplicationId");

-- Issues table
CREATE TABLE IF NOT EXISTS "Issues" (
    "Id" UUID PRIMARY KEY,
    "Title" VARCHAR(200) NOT NULL,
    "Description" TEXT NOT NULL,
    "Geom" geometry(Point, 4326),
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "Phone" VARCHAR(20),
    "Status" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "Response" TEXT,
    "RespondedAt" TIMESTAMP
);

CREATE INDEX "IX_Issues_Geom" ON "Issues" USING GIST("Geom");

-- Sales table
CREATE TABLE IF NOT EXISTS "Sales" (
    "Id" UUID PRIMARY KEY,
    "UserId" UUID NOT NULL,
    "ParcelId" UUID,
    "Geom" geometry(Polygon, 4326),
    "Price" NUMERIC(18, 2) NOT NULL,
    "Currency" VARCHAR(10) NOT NULL,
    "Description" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "ExpiresAt" TIMESTAMP NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "RenewalReminderSent" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE RESTRICT,
    FOREIGN KEY ("ParcelId") REFERENCES "Parcels"("Id") ON DELETE SET NULL
);

CREATE INDEX "IX_Sales_Geom" ON "Sales" USING GIST("Geom");
CREATE INDEX "IX_Sales_IsActive_ExpiresAt" ON "Sales"("IsActive", "ExpiresAt");

-- ScanRequests table
CREATE TABLE IF NOT EXISTS "ScanRequests" (
    "Id" UUID PRIMARY KEY,
    "ApplicantId" UUID NOT NULL,
    "Title" VARCHAR(200) NOT NULL,
    "Description" TEXT NOT NULL,
    "Geom" geometry(Point, 4326),
    "Status" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "CompletedAt" TIMESTAMP,
    FOREIGN KEY ("ApplicantId") REFERENCES "Users"("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_ScanRequests_Geom" ON "ScanRequests" USING GIST("Geom");

-- ParcelTaxStatuses table
CREATE TABLE IF NOT EXISTS "ParcelTaxStatuses" (
    "Id" UUID PRIMARY KEY,
    "ParcelId" UUID NOT NULL UNIQUE,
    "LastPaidYear" INTEGER NOT NULL,
    "YearsOverdue" INTEGER NOT NULL,
    "TotalDue" NUMERIC(18, 2) NOT NULL,
    "TotalPaid" NUMERIC(18, 2) NOT NULL,
    "Status" VARCHAR(50) NOT NULL,
    FOREIGN KEY ("ParcelId") REFERENCES "Parcels"("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ParcelTaxStatuses_ParcelId" ON "ParcelTaxStatuses"("ParcelId");

-- AuditLogs table
CREATE TABLE IF NOT EXISTS "AuditLogs" (
    "Id" UUID PRIMARY KEY,
    "ActorId" UUID NOT NULL,
    "ActionType" VARCHAR(100) NOT NULL,
    "TargetType" VARCHAR(100) NOT NULL,
    "TargetId" UUID,
    "Payload" JSONB,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX "IX_AuditLogs_ActorId_CreatedAt" ON "AuditLogs"("ActorId", "CreatedAt");
CREATE INDEX "IX_AuditLogs_TargetType_TargetId" ON "AuditLogs"("TargetType", "TargetId");

-- Function to update surface_ha when surface_m2 changes
CREATE OR REPLACE FUNCTION update_surface_ha()
RETURNS TRIGGER AS $$
BEGIN
    NEW."SurfaceHa" := NEW."SurfaceM2" / 10000.0;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_surface_ha
BEFORE INSERT OR UPDATE OF "SurfaceM2" ON "Parcels"
FOR EACH ROW
EXECUTE FUNCTION update_surface_ha();

-- Function to compute surface from geometry
CREATE OR REPLACE FUNCTION compute_parcel_surface()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW."Geom" IS NOT NULL THEN
        NEW."SurfaceM2" := ST_Area(ST_Transform(NEW."Geom", 3857));
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_compute_surface
BEFORE INSERT OR UPDATE OF "Geom" ON "Parcels"
FOR EACH ROW
EXECUTE FUNCTION compute_parcel_surface();
