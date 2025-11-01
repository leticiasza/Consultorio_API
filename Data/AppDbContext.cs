using Microsoft.EntityFrameworkCore;
using Consultorio.Models;

namespace Consultorio.Data;

public class AppDbContext : DbContext{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){}

    public  DbSet<Patient> Patients => Set<Patient>();

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
    }
}