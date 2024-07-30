using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Castor_employee.Models;

public partial class CastorEmployeeDbContext : DbContext
{
    public CastorEmployeeDbContext()
    {
    }

    public CastorEmployeeDbContext(DbContextOptions<CastorEmployeeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<JobPosition> JobPositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LAPTOP-QUEJ1BC7\\SQLEXPRESS; DataBase=CastorEmployeeDB;TrustServerCertificate=True;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC071305D4F8");

            entity.Property(e => e.FechaIngreso).HasColumnType("date");
            entity.Property(e => e.FotoPath).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdCargoNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdCargo)
                .HasConstraintName("FK__Employees__IdCar__398D8EEE");
        });

        modelBuilder.Entity<JobPosition>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__JobPosit__6C985625F0A16D07");

            entity.Property(e => e.IdCargo).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
