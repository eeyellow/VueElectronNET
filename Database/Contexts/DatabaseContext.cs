using ElectronApp.Database.Entities;
using ElectronApp.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ElectronApp.Database.Contexts
{
    /// <summary> 資料庫內容 </summary>
    public class DatabaseContext : DbContext
    {
        private static readonly bool[] MigratedRecord = { false };
        /// <summary> 帳號資料 </summary>
        public virtual DbSet<UserProfiles> UserProfiles { get; set; }
        /// <summary> 部門資料 </summary>
        public virtual DbSet<Departments> Departments { get; set; }
        /// <summary> 人員與部門的關聯表 </summary>
        public virtual DbSet<UserInDepartments> UserInDepartments { get; set; }

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="options"></param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //自動更新資料庫結構
            if (!MigratedRecord[0])
            {
                lock (MigratedRecord)
                {
                    if (!MigratedRecord[0])
                    {
                        this.Database.Migrate();
                        MigratedRecord[0] = true;
                    }
                }
            }
        }

        /// <summary>
        /// 覆寫 OnModelCreating 方法，用於設定資料庫模型建立時的行為
        /// </summary>
        /// <param name="modelBuilder">資料庫模型建立器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DatabaseContext Extensions
            SoftDeleteQueryFilter.Apply(modelBuilder);
            CustomDataTypeAttributeConvention.Apply(modelBuilder);
            DecimalPrecisionAttributeConvention.Apply(modelBuilder);
            SqlDefaultValueAttributeConvention.Apply(modelBuilder);

            // 設定資料種子
            modelBuilder.Entity<UserProfiles>().HasData(
                new UserProfiles { ID = 1, Account = "test@linkchain.tw", Name = "測試帳號", Mima = "123456" }
            );

            // Infrastructure/Seeds
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
