using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Comfort.Models;

public partial class ComfortDBContext : DbContext
{
    public ComfortDBContext()
    {
    }

    public ComfortDBContext(DbContextOptions<ComfortDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePassport> EmployeePassports { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<PartnerOffer> PartnerOffers { get; set; }

    public virtual DbSet<PartnerSalespace> PartnerSalespaces { get; set; }

    public virtual DbSet<PartnerType> PartnerTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductMaterial> ProductMaterials { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductsWorkSpace> ProductsWorkSpaces { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Workspace> Workspaces { get; set; }

    public virtual DbSet<WorkspaceType> WorkspaceTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\Hukuta\\source\\repos\\Hukuta47\\PracticalExersize2\\API Comfort\\bin\\Debug\\net10.0\\Other\\ComfortDatabase.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasOne(d => d.Passport).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PassportId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EmployeePassport>(entity =>
        {
            entity.ToTable("EmployeePassport");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.Property(e => e.Name).HasColumnType("TEXT (25)");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Offers);
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.ToTable("Partner");
        });

        modelBuilder.Entity<PartnerOffer>(entity =>
        {
            entity.ToTable("PartnerOffer");
        });

        modelBuilder.Entity<PartnerSalespace>(entity =>
        {
            entity.ToTable("PartnerSalespace");
        });

        modelBuilder.Entity<PartnerType>(entity =>
        {
            entity.ToTable("PartnerType");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasOne(d => d.ProductMaterial).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ProductMaterial>(entity =>
        {
            entity.ToTable("ProductMaterial");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType");
        });

        modelBuilder.Entity<ProductsWorkSpace>(entity =>
        {
            entity.ToTable("ProductsWorkSpace");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsWorkSpaces)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Workspace).WithMany(p => p.ProductsWorkSpaces)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Workspace>(entity =>
        {
            entity.ToTable("Workspace");

            entity.Property(e => e.NumberPeopleForProduciton).HasColumnType("INTEGER (50)");

            entity.HasOne(d => d.TypeWorkspace).WithMany(p => p.Workspaces)
                .HasForeignKey(d => d.TypeWorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<WorkspaceType>(entity =>
        {
            entity.ToTable("WorkspaceType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
