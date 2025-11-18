-- Sample seed data for PUG Platform

-- Insert sample users
-- Password for all users: "Password123!" (hashed with BCrypt)
INSERT INTO "Users" ("Id", "Email", "PasswordHash", "FirstName", "LastName", "Phone", "Role", "CreatedAt")
VALUES
    ('11111111-1111-1111-1111-111111111111', 'admin@example.com', '$2a$11$VlkDqXvZ6nfQyqBz1K5SFOqzY5F8J.Pd3vBhKsJjLlGKW.mZQKYKO', 'Admin', 'User', '+40123456789', 'SuperAdmin', NOW()),
    ('22222222-2222-2222-2222-222222222222', 'staff@example.com', '$2a$11$VlkDqXvZ6nfQyqBz1K5SFOqzY5F8J.Pd3vBhKsJjLlGKW.mZQKYKO', 'Staff', 'Member', '+40123456790', 'UrbanismStaff', NOW()),
    ('33333333-3333-3333-3333-333333333333', 'user@example.com', '$2a$11$VlkDqXvZ6nfQyqBz1K5SFOqzY5F8J.Pd3vBhKsJjLlGKW.mZQKYKO', 'John', 'Doe', '+40123456791', 'RegisteredUser', NOW());

-- Insert sample parcels with geometry
-- Parcel 1: A rectangular parcel in Bucharest area
INSERT INTO "Parcels" ("Id", "ParcelId", "OwnerId", "Geom", "SurfaceM2", "SurfaceHa", "PlotType", "Properties", "CreatedAt")
VALUES
    ('44444444-4444-4444-4444-444444444444',
     'BUC-001-2024',
     '33333333-3333-3333-3333-333333333333',
     ST_GeomFromText('POLYGON((26.0962 44.4379, 26.0972 44.4379, 26.0972 44.4369, 26.0962 44.4369, 26.0962 44.4379))', 4326),
     1250.50,
     0.1250,
     'Intravilan',
     '{"address": "Str. Exemplu 1, Bucuresti", "zone": "residential"}',
     NOW()),
    ('55555555-5555-5555-5555-555555555555',
     'BUC-002-2024',
     NULL,
     ST_GeomFromText('POLYGON((26.1062 44.4279, 26.1082 44.4279, 26.1082 44.4259, 26.1062 44.4259, 26.1062 44.4279))', 4326),
     2500.00,
     0.2500,
     'Intravilan',
     '{"address": "Str. Exemplu 2, Bucuresti", "zone": "commercial"}',
     NOW());

-- Compute surface from geometry (trigger should handle this, but let's ensure)
UPDATE "Parcels" SET "SurfaceM2" = ST_Area(ST_Transform("Geom", 3857)) WHERE "Geom" IS NOT NULL;
UPDATE "Parcels" SET "SurfaceHa" = "SurfaceM2" / 10000.0;

-- Insert sample application
INSERT INTO "Applications" ("Id", "ApplicationType", "ApplicantId", "Geom", "Status", "TrackingNumber", "CreatedAt", "UpdatedAt", "Data")
VALUES
    ('66666666-6666-6666-6666-666666666666',
     'TreeCutting',
     '33333333-3333-3333-3333-333333333333',
     ST_GeomFromText('POINT(26.0967 44.4374)', 4326),
     'Submitted',
     'TC-2024-00001',
     NOW(),
     NOW(),
     '{"reason": "Tree is diseased and poses safety risk"}');

-- Insert sample tree item for the application
INSERT INTO "TreeItems" ("Id", "ApplicationId", "Geom", "Species", "AgeEstimate", "DiameterCm", "Condition", "Motivation")
VALUES
    ('77777777-7777-7777-7777-777777777777',
     '66666666-6666-6666-6666-666666666666',
     ST_GeomFromText('POINT(26.0967 44.4374)', 4326),
     'Oak',
     70,
     45.5,
     'Diseased with visible rot',
     'Tree poses safety risk to nearby buildings and pedestrians');

-- Insert sample issue
INSERT INTO "Issues" ("Id", "Title", "Description", "Geom", "FirstName", "LastName", "Email", "Phone", "Status", "CreatedAt")
VALUES
    ('88888888-8888-8888-8888-888888888888',
     'Broken sidewalk pavement',
     'Large crack in sidewalk near intersection, safety hazard for pedestrians',
     ST_GeomFromText('POINT(26.0982 44.4364)', 4326),
     'Maria',
     'Popescu',
     'maria.popescu@example.com',
     '+40123456792',
     'New',
     NOW());

-- Insert sample sale
INSERT INTO "Sales" ("Id", "UserId", "ParcelId", "Price", "Currency", "Description", "CreatedAt", "ExpiresAt", "IsActive")
VALUES
    ('99999999-9999-9999-9999-999999999999',
     '33333333-3333-3333-3333-333333333333',
     '44444444-4444-4444-4444-444444444444',
     150000.00,
     'EUR',
     'Beautiful residential parcel in central Bucharest. Great for family home. All utilities available.',
     NOW(),
     NOW() + INTERVAL '60 days',
     TRUE);

-- Insert sample parcel tax status
INSERT INTO "ParcelTaxStatuses" ("Id", "ParcelId", "LastPaidYear", "YearsOverdue", "TotalDue", "TotalPaid", "Status")
VALUES
    (gen_random_uuid(),
     '44444444-4444-4444-4444-444444444444',
     2023,
     1,
     500.00,
     1500.00,
     'Overdue');

-- Insert audit log entry
INSERT INTO "AuditLogs" ("Id", "ActorId", "ActionType", "TargetType", "TargetId", "Payload", "CreatedAt")
VALUES
    (gen_random_uuid(),
     '33333333-3333-3333-3333-333333333333',
     'CREATE',
     'Application',
     '66666666-6666-6666-6666-666666666666',
     '{"type": "TreeCutting", "status": "Submitted"}',
     NOW());

-- Verify data
SELECT 'Users:', COUNT(*) FROM "Users";
SELECT 'Parcels:', COUNT(*) FROM "Parcels";
SELECT 'Applications:', COUNT(*) FROM "Applications";
SELECT 'TreeItems:', COUNT(*) FROM "TreeItems";
SELECT 'Issues:', COUNT(*) FROM "Issues";
SELECT 'Sales:', COUNT(*) FROM "Sales";
