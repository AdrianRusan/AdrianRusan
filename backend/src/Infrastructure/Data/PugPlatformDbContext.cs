using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using PugPlatform.Domain.Entities;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Infrastructure.Data;

public class PugPlatformDbContext : DbContext
{
    public PugPlatformDbContext(DbContextOptions<PugPlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Parcel> Parcels => Set<Parcel>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<TreeItem> TreeItems => Set<TreeItem>();
    public DbSet<DemolitionItem> DemolitionItems => Set<DemolitionItem>();
    public DbSet<ConstructionItem> ConstructionItems => Set<ConstructionItem>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<ApplicationComment> ApplicationComments => Set<ApplicationComment>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<ScanRequest> ScanRequests => Set<ScanRequest>();
    public DbSet<ParcelTaxStatus> ParcelTaxStatuses => Set<ParcelTaxStatus>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure spatial reference system (SRID 4326 for WGS84)
        modelBuilder.HasPostgresExtension("postgis");

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasConversion<string>();
        });

        // Parcel configuration
        modelBuilder.Entity<Parcel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ParcelId);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.ParcelId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Geom).HasColumnType("geometry(Polygon,4326)");
            entity.Property(e => e.SurfaceM2).HasPrecision(18, 2);
            entity.Property(e => e.SurfaceHa).HasPrecision(18, 4);
            entity.Property(e => e.PlotType).HasConversion<string>();
            entity.Property(e => e.Properties).HasColumnType("jsonb");

            entity.HasOne(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Application configuration
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TrackingNumber).IsUnique();
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.ApplicationType).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Geom).HasColumnType("geometry(Geometry,4326)");
            entity.Property(e => e.Data).HasColumnType("jsonb");
            entity.Property(e => e.TrackingNumber).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.Applicant)
                .WithMany(u => u.Applications)
                .HasForeignKey(e => e.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TreeItem configuration
        modelBuilder.Entity<TreeItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.Geom).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.Species).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DiameterCm).HasPrecision(10, 2);

            entity.HasOne(e => e.Application)
                .WithMany(a => a.TreeItems)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DemolitionItem configuration
        modelBuilder.Entity<DemolitionItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.Geom).HasColumnType("geometry(Polygon,4326)");
            entity.Property(e => e.StructureType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Material).HasMaxLength(100);

            entity.HasOne(e => e.Application)
                .WithMany(a => a.DemolitionItems)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ConstructionItem configuration
        modelBuilder.Entity<ConstructionItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.Geom).HasColumnType("geometry(Polygon,4326)");
            entity.Property(e => e.BuildingType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PlannedArea).HasPrecision(10, 2);

            entity.HasOne(e => e.Application)
                .WithMany(a => a.ConstructionItems)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Attachment configuration
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.OwnerType, e.OwnerId });
            entity.Property(e => e.OwnerType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.StoragePath).IsRequired().HasMaxLength(500);
        });

        // ApplicationComment configuration
        modelBuilder.Entity<ApplicationComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApplicationId);

            entity.HasOne(e => e.Application)
                .WithMany(a => a.Comments)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Issue configuration
        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.Geom).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status).HasConversion<string>();
        });

        // Sale configuration
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.HasIndex(e => new { e.IsActive, e.ExpiresAt });
            entity.Property(e => e.Geom).HasColumnType("geometry(Polygon,4326)");
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Currency).HasConversion<string>();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Sales)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Parcel)
                .WithMany(p => p.Sales)
                .HasForeignKey(e => e.ParcelId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ScanRequest configuration
        modelBuilder.Entity<ScanRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Geom).HasMethod("GIST");
            entity.Property(e => e.Geom).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasOne(e => e.Applicant)
                .WithMany()
                .HasForeignKey(e => e.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ParcelTaxStatus configuration
        modelBuilder.Entity<ParcelTaxStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ParcelId).IsUnique();
            entity.Property(e => e.TotalDue).HasPrecision(18, 2);
            entity.Property(e => e.TotalPaid).HasPrecision(18, 2);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.Parcel)
                .WithOne(p => p.TaxStatus)
                .HasForeignKey<ParcelTaxStatus>(e => e.ParcelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ActorId, e.CreatedAt });
            entity.HasIndex(e => new { e.TargetType, e.TargetId });
            entity.Property(e => e.ActionType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TargetType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Payload).HasColumnType("jsonb");
        });
    }
}
