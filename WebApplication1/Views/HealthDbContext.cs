using Microsoft.EntityFrameworkCore;
using HealthManagmentSystem.Models;

public class HealthDbContext : DbContext
{
    public HealthDbContext(DbContextOptions<HealthDbContext> options) : base(options) { }

    public DbSet<Admin> Admin { get; set; }
    public DbSet<Manager> Manager { get; set; }
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Login> Login { get; set; }
    public DbSet<Registration> Registration { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure decimal precision for ID fields
        modelBuilder.Entity<Manager>(entity =>
        {
            entity.Property(e => e.ID)
                  .HasColumnType("decimal(18,0)"); // Using whole numbers for IDs
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(e => e.ID)
                  .HasColumnType("decimal(18,0)"); // Using whole numbers for IDs
        });

        // Configure Login entity
        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Email);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Configure Registration entity
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Email);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired(); // Clear text password (hashed before save)
            entity.Property(e => e.PasswordHash).IsRequired(); // Hashed password
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });
    }
}

        // Configure Admin/Patient/Manager entities similarly