using Microsoft.EntityFrameworkCore;
using Consultorio.Models;

namespace Consultorio.Data;

public class AppDbContext : DbContext{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){}

    public  DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Consultation> Consultations => Set<Consultation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=consultorio.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(e =>{
            e.HasKey(p => p.Id);
            e.Property(p => p.Name);
            e.Property(p => p.BirthDate);
            e.Property(p => p.Email);
            e.HasIndex(p => p.Email).IsUnique(); // email único
            e.Property(p => p.CPF);
            e.HasIndex(p => p.CPF).IsUnique(); // CPF único
        });

        modelBuilder.Entity<Consultation>(e =>{
            e.HasKey(c => c.Id);
            e.Property(c => c.Specialty);
            e.Property(c => c.Price);
            e.Property(c => c.Date);
            
            e.HasOne(c => c.Patient)
                .WithMany(p => p.Consultations)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}