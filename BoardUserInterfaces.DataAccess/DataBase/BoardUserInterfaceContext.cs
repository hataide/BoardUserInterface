using BoardUserInterfaces.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardUserInterfaces.DataAccess.DataBase;

public class BoardUserInterfaceContext : DbContext
{
    public BoardUserInterfaceContext(DbContextOptions<BoardUserInterfaceContext> options)
    : base(options)
    {
    }

    public DbSet<FilesAudit> FilesAudits_DB { get; set; }
    public DbSet<Audits> Audits { get; set; }
    public DbSet<Logs> Logs_DB { get; set; }
    public DbSet<Files> Files_DB { get; set; }
    // ... DbSets for other tables

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Explicitly configure the primary key for the FilesAudit entity
        modelBuilder.Entity<FilesAudit>().HasKey(fa => fa.FileAuditId);
        modelBuilder.Entity<FilesAudit>()
        .Property(f => f.FileAuditId)
        .ValueGeneratedOnAdd(); // Indicates the value is generated on add.

        modelBuilder.Entity<Audits>().HasKey(f => f.AuditId);
        modelBuilder.Entity<Audits>()
        .Property(f => f.AuditId)
        .ValueGeneratedOnAdd(); // Indicates the value is generated on add.

        modelBuilder.Entity<Files>().HasKey(f => f.FileId);
        modelBuilder.Entity<Files>()
        .Property(f => f.FileId)
        .ValueGeneratedOnAdd(); // Indicates the value is generated on add.

        modelBuilder.Entity<Logs>().HasKey(f => f.LogId);
        modelBuilder.Entity<Logs>()
        .Property(f => f.LogId)
        .ValueGeneratedOnAdd(); // Indicates the value is generated on add.

    }

}