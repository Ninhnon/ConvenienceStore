using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
namespace ConvenienceStore.Models;

/// <summary>
/// 
/// </summary>
public partial class ConvenienceStoreContext : DbContext
{
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(@ConfigurationManager.ConnectionStrings["Default"].ToString());
    //}
    public ConvenienceStoreContext(DbContextOptions options) : base(options) { }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<BillDetail>().HasKey(am => new { am.BillId, am.ProductId });
    //    modelBuilder.Entity<Consignment>().HasKey(am => new { am.InputInfoId, am.ProductId });
    //    modelBuilder.Entity<Product>().HasKey(am => new { am.Barcode });
    //    base.OnModelCreating(modelBuilder);
    //}

    public ConvenienceStoreContext(DbContextOptions<ConvenienceStoreContext> options): base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<BlockVoucher> BlockVouchers { get; set; }

    public virtual DbSet<Consignment> Consignments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<InputInfo> InputInfos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<SalaryBill> SalaryBills { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=ConvenienceStore;Trusted_Connection=True;Trustservercertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bill__3214EC07FD2FDBC6");

            entity.ToTable("Bill");

            entity.Property(e => e.BillDate).HasColumnType("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CustomerId");

            entity.HasOne(d => d.User).WithMany(p => p.Bills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserId");
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => new { e.BillId, e.ProductId });

            entity.ToTable("BillDetail");

            entity.Property(e => e.ProductId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Bill).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillId");

            entity.HasOne(d => d.Product).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductId1");
        });

        modelBuilder.Entity<BlockVoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlockVou__3214EC0771C81869");

            entity.ToTable("BlockVoucher");

            entity.Property(e => e.FinishDate).HasColumnType("date");
            entity.Property(e => e.ReleaseName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");
        });

        modelBuilder.Entity<Consignment>(entity =>
        {
            entity.HasKey(e => new { e.InputInfoId, e.ProductId });

            entity.ToTable("Consignment");

            entity.Property(e => e.ProductId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnType("date");
            entity.Property(e => e.ManufacturingDate).HasColumnType("date");

            entity.HasOne(d => d.InputInfo).WithMany(p => p.Consignments)
                .HasForeignKey(d => d.InputInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InputInfoId");

            entity.HasOne(d => d.Product).WithMany(p => p.Consignments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductId");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07D5680388");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "Idx_CustomerEmail_NotNull")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Phone, "Idx_CustomerPhone_NotNull")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InputInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InputInf__3214EC07969F5E5A");

            entity.ToTable("InputInfo");

            entity.Property(e => e.InputDate).HasColumnType("date");

            entity.HasOne(d => d.Supplier).WithMany(p => p.InputInfos)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_SupplierId");

            entity.HasOne(d => d.User).WithMany(p => p.InputInfos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserId1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("PK__Product__177800D2E07A96E3");

            entity.ToTable("Product");

            entity.Property(e => e.Barcode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.ProductionSite)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.FinishDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubmittedAt).HasColumnType("smalldatetime");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.Reports)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tt");
        });

        modelBuilder.Entity<SalaryBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SalaryBi__3214EC07B6EBC1E4");

            entity.ToTable("SalaryBill");

            entity.Property(e => e.SalaryBillDate).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany(p => p.SalaryBills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserIdBill");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07453FAF98");

            entity.ToTable("Supplier");

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07EE2BC543");

            entity.HasIndex(e => e.UserName, "Idx_UserAccount_NotNull")
                .IsUnique()
                .HasFilter("([UserName] IS NOT NULL)");

            entity.HasIndex(e => e.Email, "Idx_UserEmail_NotNull")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Phone, "Idx_UserPhone_NotNull")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.Property(e => e.Avatar).HasColumnType("image");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SalaryDate).HasColumnType("date");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserRole).HasMaxLength(20);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Voucher__A25C5AA678F82D23");

            entity.ToTable("Voucher");

            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Block).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.BlockId)
                .HasConstraintName("FK__Voucher__BlockId__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
