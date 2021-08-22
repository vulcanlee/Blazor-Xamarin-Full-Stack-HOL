using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Domains.Models
{
    public partial class BackendDBContext : DbContext
    {
        public BackendDBContext()
        {
        }

        public BackendDBContext(DbContextOptions<BackendDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OrderMaster> OrderMaster { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Product> Product { get; set; }


        public virtual DbSet<MyUser> MyUser { get; set; }
        public virtual DbSet<MenuRole> MenuRole { get; set; }
        public virtual DbSet<MenuData> MenuData { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<PolicyHeader> PolicyHeader { get; set; }
        public virtual DbSet<PolicyDetail> PolicyDetail { get; set; }
        public virtual DbSet<AuditMaster> AuditItem { get; set; }
        public virtual DbSet<PhaseCategory> PhaseCategory { get; set; }
        public virtual DbSet<PhaseMessage> PhaseMessage { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=School");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            #region 設定階層級的刪除政策(預設若關聯子資料表有紀錄，父資料表不可強制刪除
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            modelBuilder.Entity<Product>()
                .Property(x => x.ListPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(x => x.ListPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(x => x.Discount)
                .HasPrecision(5, 2);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
