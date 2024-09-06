using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UMS.DAL.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Taluka> Talukas { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-EO2GF4J;Database=UMSWebApplication2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC07E8E422AD");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.State).WithMany(p => p.Districts)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK__Districts__State__2B3F6F97");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0715FE6C98");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160CF269126").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__States__3214EC074A00DDA7");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Taluka>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Talukas__3214EC07F97A2996");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.District).WithMany(p => p.Talukas)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Talukas__Distric__2E1BDC42");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07530FF615");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105340EC6AFA2").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Hobbies).HasMaxLength(255);
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.PhotoPath).HasMaxLength(255);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__UserRoles__RoleI__34C8D9D1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserRoles__UserI__33D4B598"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760ADB1B13053");
                        j.ToTable("UserRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
