using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hoarding_managment.Models
{
    public partial class db_hoarding_managementContext : DbContext
    {
       

        public db_hoarding_managementContext(DbContextOptions<db_hoarding_managementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCampaign> TblCampaigns { get; set; } = null!;
        public virtual DbSet<TblCampaignnew> TblCampaignnews { get; set; } = null!;
        public virtual DbSet<TblCampaingitem> TblCampaingitems { get; set; } = null!;
        public virtual DbSet<TblCart> TblCarts { get; set; } = null!;
        public virtual DbSet<TblCustomer> TblCustomers { get; set; } = null!;
        public virtual DbSet<TblInventory> TblInventories { get; set; } = null!;
        public virtual DbSet<TblInventoryitem> TblInventoryitems { get; set; } = null!;
        public virtual DbSet<TblPayment> TblPayments { get; set; } = null!;
        public virtual DbSet<TblQuotation> TblQuotations { get; set; } = null!;
        public virtual DbSet<TblQuotationitem> TblQuotationitems { get; set; } = null!;
        public virtual DbSet<TblReview> TblReviews { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TblVendor> TblVendors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=db_hoarding_management;user=root;password=root;persist security info=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TblCampaign>(entity =>
            {
                entity.ToTable("tbl_campaign");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookingAmt).HasMaxLength(255);

                entity.Property(e => e.CampaignNumber).HasMaxLength(45);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.FkCustomerId).HasColumnName("Fk_customerId");

                entity.Property(e => e.FkInventoryId).HasColumnName("Fk_inventoryId");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<TblCampaignnew>(entity =>
            {
                entity.ToTable("tbl_campaignnew");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasMaxLength(45)
                    .HasColumnName("amount");

                entity.Property(e => e.CampaignNumber).HasMaxLength(45);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.FkCustomerId).HasColumnName("Fk_customerId");

                entity.Property(e => e.IsDelete)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Ispaid)
                    .HasColumnName("ispaid")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<TblCampaingitem>(entity =>
            {
                entity.ToTable("tbl_campaingitem");

                entity.Property(e => e.Amount)
                    .HasMaxLength(45)
                    .HasColumnName("amount");

                entity.Property(e => e.BookingAmt).HasMaxLength(45);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.FkCampaignId).HasColumnName("Fk_CampaignId");

                entity.Property(e => e.FkInventoryId).HasColumnName("Fk_InventoryId");

                entity.Property(e => e.IsDelete)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Ispaid)
                    .HasColumnName("ispaid")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<TblCart>(entity =>
            {
                entity.ToTable("tbl_cart");

                entity.HasIndex(e => e.FkCustomerId, "idx_customer");

                entity.HasIndex(e => e.FkInventoryId, "idx_inventory");

                entity.HasIndex(e => new { e.FkCustomerId, e.FkInventoryId }, "unique_cart")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("created_by");

                entity.Property(e => e.FkCustomerId).HasColumnName("fk_customer_id");

                entity.Property(e => e.FkInventoryId).HasColumnName("fk_inventory_id");

                entity.Property(e => e.IsDelete)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_delete")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("updated_by");

                entity.HasOne(d => d.FkCustomer)
                    .WithMany(p => p.TblCarts)
                    .HasForeignKey(d => d.FkCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cart_customer");

                entity.HasOne(d => d.FkInventory)
                    .WithMany(p => p.TblCarts)
                    .HasForeignKey(d => d.FkInventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cart_inventory");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.ToTable("tbl_customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.AlternateNumber).HasMaxLength(255);

                entity.Property(e => e.BusinessName).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.ContactNo).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.CustomerName).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.GstNo).HasMaxLength(255);

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<TblInventory>(entity =>
            {
                entity.ToTable("tbl_inventory");

                entity.HasIndex(e => e.FkVendorId, "Fk_vendorId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Area).HasMaxLength(255);

                entity.Property(e => e.BookingStatus)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.FkVendorId).HasColumnName("Fk_vendorId");

                entity.Property(e => e.Height).HasMaxLength(255);

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.Rate).HasMaxLength(255);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                entity.Property(e => e.VendorAmt).HasMaxLength(255);

                entity.Property(e => e.Width).HasMaxLength(255);

                entity.HasOne(d => d.FkVendor)
                    .WithMany(p => p.TblInventories)
                    .HasForeignKey(d => d.FkVendorId)
                    .HasConstraintName("tbl_inventory_ibfk_1");
            });

            modelBuilder.Entity<TblInventoryitem>(entity =>
            {
                entity.ToTable("tbl_inventoryitems");

                entity.Property(e => e.Area).HasMaxLength(255);

                entity.Property(e => e.BookingStatus).HasColumnType("bit(1)");

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Fkcustomer).HasColumnName(" FKCustomer");

                entity.Property(e => e.Height).HasMaxLength(45);

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.Rate).HasMaxLength(45);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Width).HasMaxLength(45);
            });

            modelBuilder.Entity<TblPayment>(entity =>
            {
                entity.ToTable("tbl_payment");

                entity.HasIndex(e => e.FkCampaignId, "idx_campaign");

                entity.HasIndex(e => e.FkCustomerId, "idx_customer");

                entity.HasIndex(e => e.FkQuotationId, "idx_quotation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(12, 2)
                    .HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("created_by");

                entity.Property(e => e.FkCampaignId).HasColumnName("fk_campaign_id");

                entity.Property(e => e.FkCustomerId).HasColumnName("fk_customer_id");

                entity.Property(e => e.FkQuotationId).HasColumnName("fk_quotation_id");

                entity.Property(e => e.IsDelete)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_delete")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.PaymentStatus)
                    .HasColumnType("enum('Pending','Completed','Failed')")
                    .HasColumnName("payment_status")
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.PaymentType)
                    .HasColumnType("enum('Cash','Card','UPI','NetBanking')")
                    .HasColumnName("payment_type");

                entity.Property(e => e.TransactionRef)
                    .HasMaxLength(255)
                    .HasColumnName("transaction_ref");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("updated_by");

                entity.HasOne(d => d.FkCampaign)
                    .WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.FkCampaignId)
                    .HasConstraintName("fk_payment_campaign");

                entity.HasOne(d => d.FkCustomer)
                    .WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.FkCustomerId)
                    .HasConstraintName("fk_payment_customer");

                entity.HasOne(d => d.FkQuotation)
                    .WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.FkQuotationId)
                    .HasConstraintName("fk_payment_quotation");
            });

            modelBuilder.Entity<TblQuotation>(entity =>
            {
                entity.ToTable("tbl_quotation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.FkCustomerId).HasColumnName("Fk_CustomerId");

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.QuotationNumber).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<TblQuotationitem>(entity =>
            {
                entity.ToTable("tbl_quotationitems");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Area).HasMaxLength(255);

                entity.Property(e => e.BookingStatus).HasColumnType("bit(1)");

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.FkCustomerId).HasColumnName("Fk_CustomerId");

                entity.Property(e => e.FkInventory).HasColumnName("Fk_Inventory");

                entity.Property(e => e.FkQuotationId).HasColumnName("Fk_QuotationId");

                entity.Property(e => e.FkVendorId).HasColumnName("Fk_VendorId");

                entity.Property(e => e.Height).HasMaxLength(255);

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.LocationDescription).HasMaxLength(255);

                entity.Property(e => e.MarginAmt).HasMaxLength(255);

                entity.Property(e => e.Rate).HasMaxLength(255);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                entity.Property(e => e.VendorAmt).HasMaxLength(255);

                entity.Property(e => e.Width).HasMaxLength(255);
            });

            modelBuilder.Entity<TblReview>(entity =>
            {
                entity.ToTable("tbl_reviews");

                entity.HasIndex(e => e.FkCustomerId, "idx_customer");

                entity.HasIndex(e => e.FkInventoryId, "idx_inventory");

                entity.HasIndex(e => new { e.FkInventoryId, e.FkCustomerId }, "unique_review")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasColumnType("text")
                    .HasColumnName("comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("created_by");

                entity.Property(e => e.FkCustomerId).HasColumnName("fk_customer_id");

                entity.Property(e => e.FkInventoryId).HasColumnName("fk_inventory_id");

                entity.Property(e => e.IsDelete)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_delete")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("updated_by");

                entity.HasOne(d => d.FkCustomer)
                    .WithMany(p => p.TblReviews)
                    .HasForeignKey(d => d.FkCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_reviews_customer");

                entity.HasOne(d => d.FkInventory)
                    .WithMany(p => p.TblReviews)
                    .HasForeignKey(d => d.FkInventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_reviews_inventory");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tbl_user");

                entity.HasIndex(e => e.UserName, "UserName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);
            });

            modelBuilder.Entity<TblVendor>(entity =>
            {
                entity.ToTable("tbl_vendor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.AlternateNumber).HasMaxLength(255);

                entity.Property(e => e.BusinessName).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(45);

                entity.Property(e => e.ContactNo).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.GstNo).HasMaxLength(255);

                entity.Property(e => e.IsDelete).HasColumnType("bit(1)");

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                entity.Property(e => e.VendorName).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
