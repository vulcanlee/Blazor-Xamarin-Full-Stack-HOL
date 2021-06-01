using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.Helpers;
    using Backend.Models;
    using EFCore.BulkExtensions;
    using Entities.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ShareBusiness.Helpers;
    using System;
    using System.Linq;

    public class DatabaseInitService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }
        public IConfiguration Configuration { get; }
        public ILogger<DatabaseInitService> Logger { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public SystemLogHelper SystemLogHelper { get; }

        public DatabaseInitService(BackendDBContext context, IMapper mapper,
            IConfiguration configuration, ILogger<DatabaseInitService> logger,
            IHttpContextAccessor httpContextAccessor, SystemLogHelper systemLogHelper)
        {
            this.context = context;
            Mapper = mapper;
            Configuration = configuration;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            SystemLogHelper = systemLogHelper;
        }

        public async Task InitDataAsync()
        {
            Random random = new Random();

            #region 適用於 Code First ，刪除資料庫與移除資料庫
            string Msg = "";
            Msg = $"適用於 Code First ，刪除資料庫與移除資料庫";
            Logger.LogInformation($"{Msg}");
            await context.Database.EnsureDeletedAsync();
            Msg = $"刪除資料庫";
            Logger.LogInformation($"{Msg}");
            await context.Database.EnsureCreatedAsync();
            Msg = $"建立資料庫";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            }, () =>
            {
                Logger.LogInformation($"{Msg}");
            });
            #endregion

            #region 建立開發環境要用到的測試紀錄
            await 建立功能表角色與項目清單Async();
            Msg = $"建立功能表角色與項目清單";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            }, () =>
            {
                Logger.LogInformation($"{Msg}");
            });
            await 建立使用者紀錄Async();
            Msg = $"建立使用者紀錄";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            }, () =>
            {
                Logger.LogInformation($"{Msg}");
            });
            List<Product> products = await 建立產品紀錄Async();
            Msg = $"建立產品紀錄";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            }, () =>
            {
                Logger.LogInformation($"{Msg}");
            });
            await 建立訂單紀錄Async(random, products);
            Msg = $"建立訂單紀錄";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            }, () =>
            {
                Logger.LogInformation($"{Msg}");
            });
            #endregion
        }

        private async Task 建立訂單紀錄Async(Random random, List<Product> products)
        {
            for (int i = 0; i < 3; i++)
            {
                OrderMaster order = new OrderMaster()
                {
                    Name = $"Order{i}",
                    OrderDate = DateTime.Now.AddDays(random.Next(30)),
                    RequiredDate = DateTime.Now.AddDays(random.Next(30)),
                    ShippedDate = DateTime.Now.AddDays(random.Next(30)),
                };
                context.Add(order);
                await context.SaveChangesAsync();
                var total = random.Next(1, 6);
                for (int j = 0; j < total; j++)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        Name = $"OrderItem{j}",
                        OrderMasterId = order.Id,
                        ProductId = products[j].Id,
                        Quantity = 3,
                        ListPrice = 168,
                    };
                    context.Add(orderItem);
                }
                await context.SaveChangesAsync();
            }
            CleanTrackingHelper.Clean<Product>(context);
            CleanTrackingHelper.Clean<OrderMaster>(context);
            CleanTrackingHelper.Clean<OrderItem>(context);
        }

        private async Task<List<Product>> 建立產品紀錄Async()
        {
            CleanTrackingHelper.Clean<Product>(context);
            CleanTrackingHelper.Clean<OrderMaster>(context);
            CleanTrackingHelper.Clean<OrderItem>(context);
            List<Product> products = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product()
                {
                    Name = $"Product{i}"
                };
                products.Add(product);
                context.Add(product);
            }
            await context.SaveChangesAsync();
            return products;
        }

        #region 建立相關紀錄
        private async Task 建立使用者紀錄Async()
        {
            #region 建立使用者紀錄 

            CleanTrackingHelper.Clean<MyUser>(context);
            #region 取得各種需要的角色
            var menuRole開發者 = await context.MenuRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == MagicHelper.開發者功能表角色);
            var menuRole系統管理員 = await context.MenuRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == MagicHelper.系統管理員功能表角色);
            var menuRole使用者 = await context.MenuRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == MagicHelper.使用者功能表角色);
            #endregion

            #region 建立 開發者
            var myUser = new MyUser()
            {
                Account = $"god",
                Name = $"開發者",
                MenuRoleId = menuRole開發者.Id,
                Status = true,
                Salt = Guid.NewGuid().ToString()
            };

            var rawPassword = Configuration["AdministratorPassword"];
            myUser.Password =
                PasswordHelper.GetPasswordSHA(myUser.Salt, rawPassword);

            context.Add(myUser);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MyUser>(context);
            #endregion

            #region 建立 系統管理員
            var adminMyUser = new MyUser()
            {
                Account = $"{MagicHelper.系統管理員帳號}",
                Name = $"系統管理員 {MagicHelper.系統管理員帳號}",
                MenuRoleId = menuRole系統管理員.Id,
                Status = true,
                Salt = Guid.NewGuid().ToString()
            };
            var adminRawPassword = "123";
            adminMyUser.Password =
                PasswordHelper.GetPasswordSHA(adminMyUser.Salt, adminRawPassword);

            context.Add(adminMyUser);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MyUser>(context);
            #endregion

            #region 建立 使用者
            foreach (var item in MagicHelper.使用者帳號)
            {
                var itemMyUser = await context.MyUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == item);
                if (itemMyUser == null)
                {
                    itemMyUser = new MyUser()
                    {
                        Account = $"{item}",
                        Name = $"使用者 {item}",
                        MenuRoleId = menuRole使用者.Id,
                        Status = true,
                        Salt = Guid.NewGuid().ToString()
                    };
                    var userRawPassword = "123";
                    itemMyUser.Password =
                        PasswordHelper.GetPasswordSHA(itemMyUser.Salt, userRawPassword);

                    context.Add(itemMyUser);
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MyUser>(context);
                }
            }
            #endregion
            #endregion
        }
        private async Task 建立功能表角色與項目清單Async()
        {
            #region 建立功能表角色與項目清單

            #region 建立功能表角色
            CleanTrackingHelper.Clean<MenuRole>(context);
            #region 開發者功能表角色功能表角色
            MenuRole menuRole開發者 = new MenuRole()
            {
                Name = MagicHelper.開發者功能表角色,
                Remark = ""
            };
            context.Add(menuRole開發者);
            await context.SaveChangesAsync();
            #endregion

            #region 系統管理員功能表角色
            MenuRole menuRole系統管理員 = new MenuRole()
            {
                Name = MagicHelper.系統管理員功能表角色,
                Remark = ""
            };
            context.Add(menuRole系統管理員);
            await context.SaveChangesAsync();
            #endregion

            #region 一般使用者功能表角色
            MenuRole menuRole使用者 = new MenuRole()
            {
                Name = MagicHelper.使用者功能表角色,
                Remark = ""
            };
            context.Add(menuRole使用者);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MenuRole>(context);
            #endregion
            #endregion

            #region 建立各角色會用到的功能表清單項目

            #region 建立系統管理員角色功能表項目清單 
            CleanTrackingHelper.Clean<MenuData>(context);
            int cc = 0;

            #region 首頁功能名稱
            cc += 10;
            MenuData menuData = new MenuData()
            {
                Name = ShareBusiness.Helpers.MagicHelper.首頁功能名稱,
                CodeName = "/",
                Enable = true,
                Icon = "mdi-home",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 帳號管理功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = ShareBusiness.Helpers.MagicHelper.帳號管理功能名稱,
                CodeName = "MyUser",
                Enable = true,
                Icon = "mdi-clipboard-account",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 變更密碼
            cc += 10;
            menuData = new MenuData()
            {
                Name = ShareBusiness.Helpers.MagicHelper.變更密碼,
                CodeName = "ChangePassword",
                Enable = true,
                Icon = "mdi-form-textbox-password",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 訂單管理功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.訂單管理功能名稱,
                CodeName = "Order",
                Enable = true,
                Icon = "mdi-shopping",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 商品管理功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.商品管理功能名稱,
                CodeName = "Product",
                Enable = true,
                Icon = "mdi-gift",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 管理者專用功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.管理者專用功能名稱,
                CodeName = "OnlyAdministrator",
                Enable = true,
                Icon = "mdi-hand-pointing-up",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 一般使用者使用功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.一般使用者使用功能名稱,
                CodeName = "OnlyUser",
                Enable = true,
                Icon = "mdi-head-heart",
                IsGroup = false,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MenuData>(context);
            #endregion

            #region 系統管理員
            #region 移除 系統管理員 角色 不需要的 功能表項目清單 
            CleanTrackingHelper.Clean<MenuData>(context);
            var defaultMenuData = await context.MenuData
                .AsNoTracking()
                .Where(x => x.MenuRoleId == menuRole開發者.Id)
                .ToListAsync();

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.管理者專用功能名稱));
            #endregion

            #region 建立 系統管理員 功能表項目清單 
            foreach (var item in defaultMenuData)
            {
                item.Id = 0;
                item.MenuRoleId = menuRole系統管理員.Id;
                item.MenuRole = null;
            }
            await context.BulkInsertAsync(defaultMenuData);
            CleanTrackingHelper.Clean<MenuRole>(context);
            #endregion
            #endregion

            #region 一般使用者
            #region 移除 一般使用者 角色 不需要的 功能表項目清單 
            CleanTrackingHelper.Clean<MenuData>(context);
            defaultMenuData = await context.MenuData
                .AsNoTracking()
                .Where(x => x.MenuRoleId == menuRole開發者.Id)
                .ToListAsync();

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.帳號管理功能名稱));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.管理者專用功能名稱));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.商品管理功能名稱));

            #endregion

            #region 建立 一般使用者 功能表項目清單 
            foreach (var item in defaultMenuData)
            {
                item.Id = 0;
                item.MenuRoleId = menuRole使用者.Id;
                item.MenuRole = null;
            }
            await context.BulkInsertAsync(defaultMenuData);
            CleanTrackingHelper.Clean<MenuRole>(context);
            #endregion
            #endregion

            #endregion

            #endregion
        }
        #endregion
    }
}
