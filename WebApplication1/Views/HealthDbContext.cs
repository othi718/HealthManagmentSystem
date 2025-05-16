using Microsoft.EntityFrameworkCore;
using HealthManagmentSystem.Models;

public class HealthDbContext : DbContext
{
    public HealthDbContext(DbContextOptions<HealthDbContext> options) : base(options) { }

    public DbSet<Admin> Admin { get; set; }
    public DbSet<Manager> Manager { get; set; }
    public DbSet<Appointment> Appointment { get; set; }    // Correct type here
    public DbSet<MedicalRecord> MedicalRecord { get; set; } // Correct type here
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Login> Login { get; set; }
    public DbSet<Registration> Registration { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Admin configuration
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("decimal(18,0)");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Manager configuration
        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnType("decimal(18,0)");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Doctor configuration
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnType("decimal(18,0)");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Patient configuration
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnType("decimal(18,0)");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Login configuration
        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Email);
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Registration configuration
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Email);
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Appointment configuration
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId);
            entity.Property(e => e.DoctorId).HasColumnType("decimal(18,0)").IsRequired();
            entity.Property(e => e.PatientId).HasColumnType("decimal(18,0)").IsRequired();
            entity.Property(e => e.AppointmentDate).IsRequired();
            entity.Property(e => e.Reason).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Scheduled");

            // Relationships
            entity.HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MedicalRecord configuration
        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.ReportId);
            entity.Property(e => e.PatientId).HasColumnType("decimal(18,0)").IsRequired();
            entity.Property(e => e.DoctorId).HasColumnType("decimal(18,0)").IsRequired();
            entity.Property(e => e.Diagnosis).HasMaxLength(1000);
            entity.Property(e => e.Treatment).HasMaxLength(1000);
            entity.Property(e => e.ReportDate).IsRequired();

            // Relationships
            entity.HasOne(m => m.Patient)
                .WithMany()
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Doctor)
                .WithMany()
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
