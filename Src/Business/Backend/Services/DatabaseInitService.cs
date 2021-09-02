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

        public async Task InitDBAsync()
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
            });
            Logger.LogInformation($"{Msg}");
            #endregion
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
            });
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立開發環境要用到的測試紀錄
            #region 建立系統定義參數
            await 建立系統定義參數Async();
            Msg = $"建立系統定義參數";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            });
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立功能表角色與項目清單
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
            });
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立使用者紀錄
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
            });
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立片語分類與文字Async
            await 建立片語分類與文字Async();
            Msg = $"建立片語分類與文字";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            });
            Logger.LogInformation($"{Msg}");
            #endregion

            #region 建立簽核政策
            await 建立簽核政策Async();
            Msg = $"建立簽核政策";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.Initialization,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            });
            Logger.LogInformation($"{Msg}");
            #endregion

            #endregion
        }

        #region 建立測試紀錄
        async Task 建立片語分類與文字Async()
        {
            List<PhaseMessage> PhaseMessage = new List<PhaseMessage>();

            int cc = 10;
            #region 簽核表單使用的輸入片語
            var phaseCategory = new PhaseCategory()
            {
                Name = "簽核表單使用的輸入片語",
                Enable = true,
                OrderNumber = cc++,
                PhaseMessage = new List<PhaseMessage>()
            };
            await context.PhaseCategory.AddAsync(phaseCategory);
            await context.SaveChangesAsync();
            PhaseMessage = new List<PhaseMessage>()
                {
                    new PhaseMessage()
                    {
                        Content = "做得很好，繼續努力",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "用來形容情緒不會表露出來的人",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "像魚一樣的喝，表示喝很多，尤其指喝很多酒",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "意指非常重要、有權力或是具有影響力的人",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                };
            await context.BulkInsertAsync(PhaseMessage);
            #endregion
            #region 改善報告的輸入片語
            phaseCategory = new PhaseCategory()
            {
                Name = "改善報告的輸入片語",
                Enable = true,
                OrderNumber = cc++,
                PhaseMessage = new List<PhaseMessage>()
            };
            await context.PhaseCategory.AddAsync(phaseCategory);
            await context.SaveChangesAsync();
            PhaseMessage = new List<PhaseMessage>()
                {
                    new PhaseMessage()
                    {
                        Content = "加速意見溝通",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "○○○○案，簽會意見綜合說明如下，請鑒核",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "會簽意見一略以，○○○○○○○○○…",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "會簽意見二略以，○○○○○○○○○…",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "「是否允當」?",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "……因故不克擔任…，予以改派…",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                };
            await context.BulkInsertAsync(PhaseMessage);
            #endregion
            #region 雜項輸入片語
            phaseCategory = new PhaseCategory()
            {
                Name = "雜項輸入片語",
                Enable = true,
                OrderNumber = cc++,
                PhaseMessage = new List<PhaseMessage>()
            };
            await context.PhaseCategory.AddAsync(phaseCategory);
            await context.SaveChangesAsync();
            PhaseMessage = new List<PhaseMessage>()
                {
                    new PhaseMessage()
                    {
                        Content = "可以嗎?",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                    new PhaseMessage()
                    {
                        Content = "有關本校進修部學生向 鈞部「部長信箱」反映課程標準一案，本校已查明原委，謹檢陳查核報告乙份（如附件），敬請 鑒核。",
                        Enable = true,
                        Code = cc.ToString("D5"),
                        OrderNumber = cc++,
                        PhaseCategoryId = phaseCategory.Id,
                    },
                };
            await context.BulkInsertAsync(PhaseMessage);
            #endregion
        }

        async Task 建立簽核政策Async()
        {
            List<PolicyDetail> policyDetail = new List<PolicyDetail>();
            var allUsers = await context.MyUser.ToListAsync();
            #region 稽核室簽核流程
            var policyHeader = new PolicyHeader()
            {
                Name = "稽核室簽核流程",
                Enable = true,
            };
            await context.PolicyHeader.AddAsync(policyHeader);
            await context.SaveChangesAsync();
            policyDetail = new List<PolicyDetail>()
                {
                    new PolicyDetail()
                    {
                        Name = "主任",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user10").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user11").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "副課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user12").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "經理",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user13").Id,
                    },
                };
            await context.BulkInsertAsync(policyDetail);
            #endregion

            #region 稽核室簽核流程(都有副本)
            policyHeader = new PolicyHeader()
            {
                Name = "稽核室簽核流程(都有副本)",
                Enable = true,
            };
            await context.PolicyHeader.AddAsync(policyHeader);
            await context.SaveChangesAsync();
            policyDetail = new List<PolicyDetail>()
                {
                    new PolicyDetail()
                    {
                        Name = "主任",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user10").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "組長",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user1").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user11").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "副課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user12").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "經理",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user13").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "總經理",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user13").Id,
                    },
                };
            await context.BulkInsertAsync(policyDetail);
            #endregion

            #region 稽核室簽核流程(有副本與會簽)
            policyHeader = new PolicyHeader()
            {
                Name = "稽核室簽核流程(有副本與會簽)",
                Enable = true,
            };
            await context.PolicyHeader.AddAsync(policyHeader);
            await context.SaveChangesAsync();
            policyDetail = new List<PolicyDetail>()
                {
                    new PolicyDetail()
                    {
                        Name = "主任",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user10").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "組長",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user1").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user11").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "總務課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user14").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "副課長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user12").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "經理",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user13").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "總經理",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user13").Id,
                    },
                };
            await context.BulkInsertAsync(policyDetail);
            #endregion

            #region 異常問題簽核流程
            policyHeader = new PolicyHeader()
            {
                Name = "異常問題簽核流程",
                Enable = true,
            };
            await context.PolicyHeader.AddAsync(policyHeader);
            await context.SaveChangesAsync();
            policyDetail = new List<PolicyDetail>()
                {
                    new PolicyDetail()
                    {
                        Name = "現場領班",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user15").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "廠長",
                        Enable = true,
                        Level = 2,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user16").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "總經理",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=true,
                        MyUserId = allUsers.First(x=>x.Account=="user17").Id,
                    },
                    new PolicyDetail()
                    {
                        Name = "處長",
                        Enable = true,
                        Level = 3,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user18").Id,
                    },
                };
            await context.BulkInsertAsync(policyDetail);
            #endregion

            #region 一般事項簽核流程
            policyHeader = new PolicyHeader()
            {
                Name = "一般事項簽核流程",
                Enable = true,
            };
            await context.PolicyHeader.AddAsync(policyHeader);
            await context.SaveChangesAsync();
            policyDetail = new List<PolicyDetail>()
                {
                    new PolicyDetail()
                    {
                        Name = "主任",
                        Enable = true,
                        Level = 1,
                        PolicyHeaderId = policyHeader.Id,
                        OnlyCC=false,
                        MyUserId = allUsers.First(x=>x.Account=="user10").Id,
                    },
                };
            await context.BulkInsertAsync(policyDetail);
            #endregion

        }

        #endregion

        #region 建立相關紀錄
        private async Task 建立系統定義參數Async()
        {
            #region 建立系統定義參數 

            CleanTrackingHelper.Clean<SystemEnvironment>(context);
            #region 新增系統定義紀錄
            SystemEnvironment systemEnvironment = new SystemEnvironment()
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

            context.SystemEnvironment.Add(systemEnvironment);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<SystemEnvironment>(context);
            #endregion
            #endregion
        }
        private async Task 建立使用者紀錄Async()
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

            #region 系統資料管理子功能表
            cc += 10;
            menuData = new MenuData()
            {
                Name = "系統資料管理",
                CodeName = "",
                Enable = true,
                Icon = "mdi-star-box",
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
                Name = MagicHelper.系統運作條件,
                CodeName = "Environment",
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

            #region 簽核流程政策
            cc += 10;
            menuData = new MenuData()
            {
                Name = MagicHelper.簽核流程政策,
                CodeName = "Policy",
                Enable = true,
                Icon = "mdi-head-heart",
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

            #region 基本資料管理子功能表
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

            #region 片語分類
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.片語分類,
                CodeName = "PhaseCategory",
                Enable = true,
                Icon = "mdi-lightbulb-group",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 簽核文件子功能表
            cc += 10;
            menuData = new MenuData()
            {
                Name = "簽核管理",
                CodeName = "",
                Enable = true,
                Icon = "mdi-file-document-multiple",
                IsGroup = true,
                Level = 0,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 工單
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.派工單,
                CodeName = "WorkOrder",
                Enable = true,
                Icon = "mdi-cog-transfer-outline",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 簽核文件
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.簽核文件,
                CodeName = "Flow",
                Enable = true,
                Icon = "mdi-file-document-edit",
                IsGroup = false,
                Level = 1,
                MenuRoleId = menuRole開發者.Id,
                Sequence = cc,
            };
            context.Add(menuData);
            #endregion

            #region 簽核收件匣
            cc += 10;
            menuData = new MenuData()
            {
                Name = BAL.Helpers.MagicHelper.簽核收件匣,
                CodeName = "FlowInbox",
                Enable = true,
                Icon = "mdi-email-outline",
                IsGroup = false,
                Level = 1,
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
                .FirstOrDefault(x => x.Name == MagicHelper.系統運作條件));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.帳號管理功能名稱));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.簽核流程政策));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.郵件寄送紀錄));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.系統訊息廣播));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == "基本資料管理"));

            defaultMenuData
                .Remove(defaultMenuData
                .FirstOrDefault(x => x.Name == MagicHelper.片語分類));

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
