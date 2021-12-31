using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.Helpers;
    using Backend.Models;
    using EFCore.BulkExtensions;
    using Domains.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using BAL.Helpers;
    using System;
    using System.Linq;
    using Newtonsoft.Json;

    public class DatabaseInitService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }
        public IConfiguration Configuration { get; }
        public ILogger<DatabaseInitService> Logger { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public SystemLogHelper SystemLogHelper { get; }
        public InitDatas InitDatas { get; set; } = new InitDatas();

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

        public async Task InitDBAsync(string path, Action<string> OnUpdateMessage)
        {
            string content = await File.ReadAllTextAsync(path);
            InitDatas = JsonConvert.DeserializeObject<InitDatas>(content);
            Random random = new Random();

            context.Database.SetCommandTimeout(TimeSpan.FromMinutes(3));

            #region 適用於 Code First ，刪除資料庫與移除資料庫
            string Msg = "";
            Msg = $"適用於 Code First ，刪除資料庫與移除資料庫";
            OnUpdateMessage(Msg);
            Logger.LogInformation($"{Msg}");
            await context.Database.EnsureDeletedAsync();
            Msg = $"刪除資料庫";
            OnUpdateMessage(Msg);
            Logger.LogInformation($"{Msg}");
            await context.Database.EnsureCreatedAsync();
            Msg = $"建立資料庫";
            OnUpdateMessage(Msg);
            try
            {
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.Initialization,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
            }
            catch (Exception)
            {
            }
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 還原預設紀錄
            DateTime currentNow;
            #region AccountPolicy
            currentNow = DateTime.Now;
            Msg = $"建立 AccountPolicy";
            OnUpdateMessage(Msg);
            await context.AccountPolicy.AddRangeAsync(InitDatas.AccountPolicy);
            await context.SaveChangesWithoutIdentityInsertAsync<AccountPolicy>();
            OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            #endregion

            #region MenuRole
            currentNow = DateTime.Now;
            Msg = $"建立 MenuRole";
            OnUpdateMessage(Msg);
            await context.MenuRole.AddRangeAsync(InitDatas.MenuRole);
            await context.SaveChangesWithoutIdentityInsertAsync<MenuRole>();
            OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            #endregion

            #region MenuData
            currentNow = DateTime.Now;
            Msg = $"建立 MenuData";
            OnUpdateMessage(Msg);
            await context.MenuData.AddRangeAsync(InitDatas.MenuData);
            await context.SaveChangesWithoutIdentityInsertAsync<MenuData>();
            OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            #endregion

            #region MyUser
            currentNow = DateTime.Now;
            Msg = $"建立 MyUser";
            OnUpdateMessage(Msg);
            await context.MyUser.AddRangeAsync(InitDatas.MyUser);
            await context.SaveChangesWithoutIdentityInsertAsync<MyUser>();
            OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            #endregion

            #region MyUserPasswordHistory
            currentNow = DateTime.Now;
            Msg = $"建立 MyUserPasswordHistory";
            OnUpdateMessage(Msg);
            await context.MyUserPasswordHistory.AddRangeAsync(InitDatas.MyUserPasswordHistory);
            await context.SaveChangesWithoutIdentityInsertAsync<MyUserPasswordHistory>();
            OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            #endregion
            #endregion
        }

        public async Task InitDataAsync(string InitializationMode, Action<string> OnUpdateMessage)
        {
            Random random = new Random();
            DateTime currentNow = DateTime.Now;

            context.Database.SetCommandTimeout(TimeSpan.FromMinutes(3));

            #region 適用於 Code First ，刪除資料庫與移除資料庫
            string Msg = "";
            Msg = $"適用於 Code First ，刪除資料庫與移除資料庫";
            OnUpdateMessage(Msg);
            Logger.LogInformation($"{Msg}");
            try
            {
                currentNow = DateTime.Now;
                Msg = $"刪除資料庫";
                OnUpdateMessage($"{Msg}");
                await context.Database.EnsureDeletedAsync();
                OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
                Logger.LogInformation($"{Msg} ({DateTime.Now - currentNow})");
            }
            catch (Exception ex)
            {
                OnUpdateMessage($"{Msg} 發生例外異常 ({DateTime.Now - currentNow})");
                OnUpdateMessage(ex.Message);
                Logger.LogError(Msg, ex);
                return;
            }
            try
            {
                currentNow = DateTime.Now;
                Msg = $"建立資料庫";
                OnUpdateMessage($"{Msg}");
                await context.Database.EnsureCreatedAsync();
                OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
                Logger.LogInformation($"{Msg} ({DateTime.Now - currentNow})");
            }
            catch (Exception ex)
            {
                OnUpdateMessage($"{Msg} 發生例外異常 ({DateTime.Now - currentNow})");
                OnUpdateMessage(ex.Message);
                Logger.LogError(Msg, ex);
                return;
            }
            try
            {
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.Initialization,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
            }
            catch (Exception)
            {
            }
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立開發環境要用到的測試紀錄
            #region 建立系統定義參數
            try
            {
                currentNow = DateTime.Now;
                Msg = $"建立系統定義參數";
                OnUpdateMessage(Msg);
                await 建立系統定義參數Async();
                OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            }
            catch (Exception ex)
            {
                OnUpdateMessage($"{Msg} 發生例外異常 ({DateTime.Now - currentNow})");
                OnUpdateMessage(ex.Message);
                Logger.LogError(Msg, ex);
                return;
            }

            try
            {
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.Initialization,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
            }
            catch (Exception)
            {
            }
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立功能表角色與項目清單
            try
            {
                currentNow = DateTime.Now;
                Msg = $"建立功能表角色與項目清單";
                OnUpdateMessage(Msg);
                await 建立功能表角色與項目清單Async();
                OnUpdateMessage($"{Msg} ({DateTime.Now - currentNow})");
            }
            catch (Exception ex)
            {
                OnUpdateMessage($"{Msg} 發生例外異常 ({DateTime.Now - currentNow})");
                OnUpdateMessage(ex.Message);
                Logger.LogError(Msg, ex);
                return;
            }
            try
            {
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.Initialization,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
            }
            catch (Exception)
            {
            }
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立使用者紀錄
            await 建立使用者紀錄Async(InitializationMode);
            Msg = $"建立使用者紀錄";
            try
            {
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.Initialization,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
            }
            catch (Exception)
            {
            }
            Logger.LogInformation($"{Msg}");
            #endregion

            if (InitializationMode == "開發模式")
            {
            }
            #endregion

            Msg = $"資料庫初始化作業完成";
            OnUpdateMessage(Msg);
        }

        #region 建立測試紀錄
        #endregion

        #region 建立相關紀錄
        private async Task 建立系統定義參數Async()
        {
            #region 建立系統定義參數 

            CleanTrackingHelper.Clean<AccountPolicy>(context);
            #region 新增系統定義紀錄
            AccountPolicy AccountPolicy = new AccountPolicy()
            {
                EnableLoginFailDetection = true,
                LoginFailMaxTimes = 3,
                LoginFailTimesLockMinutes = 5,
                MinimumPasswordLength = 3,
                PasswordAge = 42,
                PasswordComplexity = 3,  // PasswordStrength.Medium
                PasswordHistory = 20,
                EnableCheckPasswordAge = true,
            };

            context.AccountPolicy.Add(AccountPolicy);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<AccountPolicy>(context);
            #endregion
            #endregion
        }
        private async Task 建立使用者紀錄Async(string InitializationMode)
        {
            #region 建立使用者紀錄 

            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
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
                Salt = Guid.NewGuid().ToString(),
                ForceLogoutDatetime = DateTime.Now.AddDays(-1),
                ForceChangePassword = false,
                ForceChangePasswordDatetime = DateTime.Now.AddDays(42),
                LoginFailTimes = 0,
                LoginFailUnlockDatetime = DateTime.Now.AddDays(-1),
                LastLoginDatetime = DateTime.Now,
                Email = "vulcan.lee@gmail.com",
            };

            myUser.Salt = Guid.NewGuid().ToString();
            myUser.Password =
             PasswordHelper.GetPasswordSHA(myUser.Salt + "Vulcan", "abc");

            context.Add(myUser);
            await context.SaveChangesAsync();

            MyUserPasswordHistory myUserPasswordHistoryAdapterModel = new MyUserPasswordHistory()
            {
                MyUserId = myUser.Id,
                IP = "",
                Password = myUser.Password,
                ChangePasswordDatetime = DateTime.Now,
            };

            await context.AddAsync(myUserPasswordHistoryAdapterModel);
            await context.SaveChangesAsync();

            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
            #endregion

            #region 建立 系統管理員
            var adminMyUser = new MyUser()
            {
                Account = $"{MagicHelper.系統管理員帳號}",
                Name = $"系統管理員 {MagicHelper.系統管理員帳號}",
                MenuRoleId = menuRole系統管理員.Id,
                Status = true,
                Salt = Guid.NewGuid().ToString(),
                ForceLogoutDatetime = DateTime.Now.AddDays(-1),
                ForceChangePassword = false,
                ForceChangePasswordDatetime = DateTime.Now.AddDays(42),
                LoginFailTimes = 0,
                LoginFailUnlockDatetime = DateTime.Now.AddDays(-1),
                LastLoginDatetime = DateTime.Now,
                Email = "vulcan.lee@gmail.com",
            };
            var adminRawPassword = "123";
            adminMyUser.Password =
                PasswordHelper.GetPasswordSHA(adminMyUser.Salt, adminRawPassword);

            context.Add(adminMyUser);
            await context.SaveChangesAsync();

            myUserPasswordHistoryAdapterModel = new MyUserPasswordHistory()
            {
                MyUserId = adminMyUser.Id,
                IP = "",
                Password = adminMyUser.Password,
                ChangePasswordDatetime = DateTime.Now,
            };

            await context.AddAsync(myUserPasswordHistoryAdapterModel);
            await context.SaveChangesAsync();

            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
            #endregion

            if (InitializationMode == "開發模式")
            {
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
                            Salt = Guid.NewGuid().ToString(),
                            ForceLogoutDatetime = DateTime.Now.AddDays(-1),
                            ForceChangePassword = false,
                            ForceChangePasswordDatetime = DateTime.Now.AddDays(42),
                            LoginFailTimes = 0,
                            LoginFailUnlockDatetime = DateTime.Now.AddDays(-1),
                            LastLoginDatetime = DateTime.Now,
                            Email = "vulcan.lee@gmail.com",
                        };
                        var userRawPassword = "123";
                        itemMyUser.Password =
                            PasswordHelper.GetPasswordSHA(itemMyUser.Salt, userRawPassword);

                        context.Add(itemMyUser);
                        await context.SaveChangesAsync();

                        myUserPasswordHistoryAdapterModel = new MyUserPasswordHistory()
                        {
                            MyUserId = itemMyUser.Id,
                            IP = "",
                            Password = itemMyUser.Password,
                            ChangePasswordDatetime = DateTime.Now,
                        };
                        await context.AddAsync(myUserPasswordHistoryAdapterModel);
                        await context.SaveChangesAsync();

                        CleanTrackingHelper.Clean<MyUser>(context);
                        CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                    }
                }
                #endregion
            }
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
                Name = BAL.Helpers.MagicHelper.首頁功能名稱,
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

            #region 變更密碼
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.變更密碼,
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

            #region 系統資料管理   子功能表
            #region 系統資料管理   子功能表
            cc += 10;
            menuData = new MenuData()
            {
                Name = "系統資料管理",
                CodeName = "",
                Enable = true,
                Icon = "mdi-power-plug",
                IsGroup = true,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 系統運作條件
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.帳號密碼政策,
                CodeName = "AccoutnPolicy",
                Enable = true,
                Icon = "mdi-cog",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 帳號管理功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.帳號管理功能名稱,
                CodeName = "MyUser",
                Enable = true,
                Icon = "mdi-clipboard-account",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 郵件寄送紀錄
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.郵件寄送紀錄,
                CodeName = "MailQueue",
                Enable = true,
                Icon = "mdi-email-send",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 系統訊息廣播
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.系統訊息廣播,
                CodeName = "Broadcast",
                Enable = true,
                Icon = "mdi-message-alert",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region App例外異常
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.App例外異常,
                CodeName = "ExceptionRecord",
                Enable = true,
                Icon = "mdi-message-alert",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion
            #endregion

            #region 基本資料管理   子功能表
            #region 基本資料管理
            cc += 10;
            menuData = new MenuData()
            {
                Name = "基本資料管理",
                CodeName = "",
                Enable = true,
                Icon = "mdi-database-cog",
                IsGroup = true,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 範例程式碼，隱藏其功能表清單
            #region 商品管理功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.商品管理功能名稱,
                CodeName = "Product",
                Enable = true,
                Icon = "mdi-lightbulb-group",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 商品管理功能名稱
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.訂單管理功能名稱,
                CodeName = "Order",
                Enable = true,
                Icon = "mdi-lightbulb-group",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion
            #endregion
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
                .FirstOrDefault(x => x.Name == "系統資料管理"));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.帳號密碼政策));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == "基本資料管理"));

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
