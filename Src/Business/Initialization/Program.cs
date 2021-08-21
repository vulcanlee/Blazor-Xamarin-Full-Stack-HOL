using Domains.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BAL.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Initialization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region 取得 Blazor 專案內的連線字串
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            string connectionString = config.GetConnectionString(MagicHelper.DefaultConnectionString);
            #endregion

            #region 使用取得的連線字串，建立 DbContext
            DbContextOptions<BackendDBContext> options = new DbContextOptionsBuilder<BackendDBContext>()
                .UseSqlServer(connectionString)
                .Options;
            BackendDBContext context = new BackendDBContext(options);
            #endregion

            Random random = new Random();
            Console.WriteLine("請輸入任意鍵後");
            Console.WriteLine("開始執行 Database Initialization");
            Console.ReadKey();

            #region 適用於 Code First ，刪除資料庫與移除資料庫
            Console.WriteLine($"請稍後，刪除資料庫中");
            await context.Database.EnsureDeletedAsync();
            Console.WriteLine($"請稍後，建立資料庫中");
            await context.Database.EnsureCreatedAsync();
            #endregion

            await GenerateTestDataAsync(context, random);
        }

        private static async Task GenerateTestDataAsync(BackendDBContext context, Random random)
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            #region 產生開發用的測試資料
            //await GenerateMyUserAsync(context, random);
            #endregion
        }

        private static async Task GenerateMyUserAsync(BackendDBContext context, Random random)
        {
            List<MyUser> users = new List<MyUser>();
            for (int i = 1; i < 30; i++)
            {
                MyUser MyUser = new MyUser()
                {
                    Account = $"user{i}",
                    Name = $"user{i}",
                    Password = "pw",
                };
                users.Add(MyUser);
            }
            await context.BulkInsertAsync(users);
        }
    }
}
