using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bookstore.Entity
{
    public partial class dbBookstoreContext : DbContext
    {
        public dbBookstoreContext()
        {
        }

        public dbBookstoreContext(DbContextOptions<dbBookstoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAdministration> TbAdministrations { get; set; } = null!;
        public virtual DbSet<TbBanner> TbBanners { get; set; } = null!;
        public virtual DbSet<TbBook> TbBooks { get; set; } = null!;
        public virtual DbSet<TbBookComment> TbBookComments { get; set; } = null!;
        public virtual DbSet<TbBookRating> TbBookRatings { get; set; } = null!;
        public virtual DbSet<TbBookType> TbBookTypes { get; set; } = null!;
        public virtual DbSet<TbCustomer> TbCustomers { get; set; } = null!;
        public virtual DbSet<TbLocation> TbLocations { get; set; } = null!;
        public virtual DbSet<TbOrder> TbOrders { get; set; } = null!;
        public virtual DbSet<TbOrderDetail> TbOrderDetails { get; set; } = null!;
        public virtual DbSet<TbPost> TbPosts { get; set; } = null!;
        public virtual DbSet<TbPublisher> TbPublishers { get; set; } = null!;
        public virtual DbSet<TbRole> TbRoles { get; set; } = null!;
        public virtual DbSet<TbTransactStatus> TbTransactStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NMHAO\\SQLEXPRESS;Database=dbBookstore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbAdministration>(entity =>
            {
                entity.HasKey(e => e.AdministrationId);

                entity.ToTable("tb_Administration");

                entity.Property(e => e.AdministrationId).HasColumnName("AdministrationID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TbAdministrations)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_tb_Administration_tb_Role");
            });

            modelBuilder.Entity<TbBanner>(entity =>
            {
                entity.HasKey(e => e.BannerId);

                entity.ToTable("tb_Banner");

                entity.Property(e => e.BannerId).HasColumnName("BannerID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsNewfeed).HasColumnName("isNewfeed");

                entity.Property(e => e.ListImage).HasMaxLength(250);

                entity.Property(e => e.SubTitle).HasMaxLength(150);

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.Property(e => e.UrlLink).HasMaxLength(250);
            });

            modelBuilder.Entity<TbBook>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.ToTable("tb_Book");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.BookName).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.IsHot).HasColumnName("isHot");

                entity.Property(e => e.Language).HasMaxLength(250);

                entity.Property(e => e.ListImage).HasMaxLength(500);

                entity.Property(e => e.PublishYear).HasColumnName("Publish Year");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.Translator).HasMaxLength(50);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.TbBooks)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_tb_Book_tb_Publisher");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TbBooks)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_tb_Book_tb_BookType");
            });

            modelBuilder.Entity<TbBookComment>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.ToTable("tb_BookComment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbBookComments)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_BookComment_tb_Book");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TbBookComments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_BookComment_tb_Customer");
            });

            modelBuilder.Entity<TbBookRating>(entity =>
            {
                entity.HasKey(e => e.RateId);

                entity.ToTable("tb_BookRating");

                entity.Property(e => e.RateId).HasColumnName("RateID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbBookRatings)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_tb_BookRating_tb_Book");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TbBookRatings)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_tb_BookRating_tb_Customer");
            });

            modelBuilder.Entity<TbBookType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("tb_BookType");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.TypeName).HasMaxLength(250);
            });

            modelBuilder.Entity<TbCustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("tb_Customer");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FullName).HasMaxLength(30);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(8)
                    .IsFixedLength();

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TbCustomers)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tb_Customer_tb_Location");
            });

            modelBuilder.Entity<TbLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("tb_Location");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameWithType).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(100);

                entity.Property(e => e.Type).HasMaxLength(10);
            });

            modelBuilder.Entity<TbOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("tb_Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_tb_Order_tb_Customer");

                entity.HasOne(d => d.TransactStatus)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.TransactStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_Order_tb_TransactStatus");
            });

            modelBuilder.Entity<TbOrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);

                entity.ToTable("tb_OrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbOrderDetails)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_tb_OrderDetail_tb_Book");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TbOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tb_OrderDetail_tb_Order");
            });

            modelBuilder.Entity<TbPost>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.ToTable("tb_Post");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsHot).HasColumnName("isHot");

                entity.Property(e => e.IsNewfeed).HasColumnName("isNewfeed");

                entity.Property(e => e.Scontents)
                    .HasMaxLength(250)
                    .HasColumnName("SContents");

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<TbPublisher>(entity =>
            {
                entity.HasKey(e => e.PublisherId);

                entity.ToTable("tb_Publisher");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.PublisherName).HasMaxLength(250);
            });

            modelBuilder.Entity<TbRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("tb_Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<TbTransactStatus>(entity =>
            {
                entity.HasKey(e => e.TransactStatusId);

                entity.ToTable("tb_TransactStatus");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.Property(e => e.Desription).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
