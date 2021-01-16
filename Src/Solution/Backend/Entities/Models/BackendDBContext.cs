using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Entities.Models
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

        public virtual DbSet<Holuser> Holusers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<MyUser> MyUsers { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<LeaveCategory> LeaveCategories { get; set; }
        public virtual DbSet<OnCallPhone> OnCallPhones { get; set; }
        public virtual DbSet<LeaveForm> LeaveForms { get; set; }
        public virtual DbSet<WorkingLog> WorkingLogs { get; set; }
        public virtual DbSet<WorkingLogDetail> WorkingLogDetails { get; set; }
        public virtual DbSet<TravelExpense> TravelExpenses { get; set; }
        public virtual DbSet<TravelExpenseDetail> TravelExpenseDetails { get; set; }


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
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            #region 設定階層級的刪除政策(預設若關聯子資料表有紀錄，父資料表不可強制刪除
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            modelBuilder.Entity<TravelExpense>()
                .Property(x => x.TotalExpense)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TravelExpenseDetail>()
                .Property(x => x.Expense)
                .HasPrecision(13, 2);

            modelBuilder.Entity<WorkingLog>()
                .Property(x => x.TotalHours)
                .HasPrecision(4, 1);

            modelBuilder.Entity<WorkingLogDetail>()
                .Property(x => x.Hours)
                .HasPrecision(4, 1);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
