using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using ShareBusiness.Helpers;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using ShareDomain.DataModels;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using AutoMapper;
    using ShareBusiness.Factories;
    using ShareDomain.Enums;

    public class DatabaseInitService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public DatabaseInitService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task InitDataAsync()
        {
            #region 適用於 Code First ，刪除資料庫與移除資料庫
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            #endregion

            #region 使用者 
            var items = Get姓名();
            CleanTrackingHelper.Clean<MyUser>(context);
            int idx = 1;
            foreach (var item in items)
            {
                var itemMyUser = await context.MyUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == item);
                if (itemMyUser == null)
                {
                    itemMyUser = new MyUser()
                    {
                        Account = $"user{idx}",
                        Name = $"{item}",
                        Password = "pw",
                    };
                    if (idx == 9) itemMyUser.IsManager = true;
                    context.Add(itemMyUser);
                    await context.SaveChangesAsync();
                    idx++;
                }
            }
            CleanTrackingHelper.Clean<MyUser>(context);
            #endregion
            #region 假別
            items = Get假別();
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            int order = 0;
            foreach (var item in items)
            {
                var itemLeaveCategory = await context.LeaveCategory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == item);
                if (itemLeaveCategory == null)
                {
                    order += 10;
                    itemLeaveCategory = new LeaveCategory()
                    {
                        Name = item,
                        OrderNumber = order,
                    };
                    context.Add(itemLeaveCategory);
                    await context.SaveChangesAsync();
                }
            }
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            #endregion
            #region 連絡電話
            var items連絡電話 = Get連絡電話();
            CleanTrackingHelper.Clean<OnCallPhone>(context);
            order = 0;
            foreach ((string title, string phone) in items連絡電話)
            {
                var item連絡電話 = await context.OnCallPhone
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Title == title);
                if (item連絡電話 == null)
                {
                    order += 10;
                    item連絡電話 = new OnCallPhone()
                    {
                        OrderNumber = order,
                        Title = title,
                        PhoneNumber = phone,
                    };
                    context.Add(item連絡電話);
                    await context.SaveChangesAsync();
                }
            }
            CleanTrackingHelper.Clean<OnCallPhone>(context);
            #endregion
            #region 專案
            items = Get專案();
            CleanTrackingHelper.Clean<Project>(context);
            foreach (var item in items)
            {
                var itemProject = await context.Project
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == item);
                if (itemProject == null)
                {
                    itemProject = new Project()
                    {
                        Name = item,
                    };
                    context.Add(itemProject);
                    await context.SaveChangesAsync();
                }
            }
            CleanTrackingHelper.Clean<Project>(context);
            #endregion
        }

        public List<string> Get姓名()
        {
            string nameString = "邱明儒、連寧甫、張瑞信、黃素貞、林淑惠、林國瑋、楊逸群、黃思翰、施詩婷、許建勳、蔡明虹、車振宇、李政心、王秀美、陳彥廷、袁幸萍、程怡君、林育齊、梁禎火、游登冰、袁彥璋、謝其易、楊宗翰、林欣年、王博新、林家希、陳奕翔、王萱寧、馮哲維、張瑤任、杜子方、曹松隆、張瑞發、陳俐瑜、侯宇嘉、翁俊毅、陳奕海、吳千慧、何茂平、郭又蕙、蘇志忠、藍邦琬、黃慧珠、夏姿瑩、劉柏鈞、張剛伸、顏彥文、張嘉玫、楊仕榮、馬育萱、王威任、林佳文、張淑敏、王俊剛、陳仕妍、黃宗吟、黃俊音、王志成、林郁人、王偉傑、張麗芬、郭欣怡、謝明淑、彭靜芳、吳淑華、周志星、黃美堅、盧哲瑋、吳朝以、張惠文、林秀玲、韓國偉、許靜宜、郭進瑤、符佩琪、張庭瑋、周聖凱";
            List<string> all姓名 = new List<string>();
            var allNames = nameString.Split("、");
            foreach (var item in allNames)
            {
                all姓名.Add(item);
            }
            return all姓名;
        }

        public List<string> Get假別()
        {
            List<string> all假別 = new List<string>();
            all假別.Add("病假(一般病假)");
            all假別.Add("病假(住院病假)");
            all假別.Add("特別休假");
            all假別.Add("事假");
            all假別.Add("事假(家庭照顧假)");
            all假別.Add("生理假");
            all假別.Add("婚假");
            all假別.Add("喪假");
            all假別.Add("公假");
            all假別.Add("公傷病假");
            all假別.Add("產假含例假日");
            all假別.Add("安胎假");
            all假別.Add("撫育假");
            return all假別;
        }

        public List<string> Get專案()
        {
            List<string> all專案 = new List<string>();
            all專案.Add("Blazor CMS");
            all專案.Add("ASP.NET Core Web API");
            all專案.Add("Xamarin.Forms");
            all專案.Add("Blazor Deployment");
            all專案.Add("Android Publish");
            all專案.Add("iOS Publish");
            return all專案;
        }

        public List<(string, string)> Get連絡電話()
        {
            List<(string, string)> all連絡電話 = new List<(string, string)>();
            all連絡電話.Add(("警衛室", "+886091234560"));
            all連絡電話.Add(("行政部 Administrative Department", "+886091234561"));
            all連絡電話.Add(("稽核室 Auditorial Room", "+88609123452"));
            all連絡電話.Add(("董事長室 Chairman's Office", "+88609123453"));
            all連絡電話.Add(("電腦中心 Computer Center", "+88609123454"));
            all連絡電話.Add(("客戶服務部 Customer Service Department", "+88609123455"));
            all連絡電話.Add(("財務部 Finance Department", "+88609123456"));
            all連絡電話.Add(("管理部 Financial & Administrative Department", "+88609123457"));
            all連絡電話.Add(("總務部 General Affairs Department", "+88609123458"));
            all連絡電話.Add(("人力資源部 Human Resources Department", "+88609123459"));
            all連絡電話.Add(("資訊部 IT Department", "+88609123460"));
            all連絡電話.Add(("行銷部 Marketing Department", "+8860912361"));
            all連絡電話.Add(("企劃部 Planning Department", "+88609123462"));
            all連絡電話.Add(("採購部 Procurement Department", "+88609123463"));
            all連絡電話.Add(("品管部 Quality Control Department", "+88609123464"));
            all連絡電話.Add(("研究開發部 Research & Development Department", "+88609123465"));
            all連絡電話.Add(("業務部 Sales Department", "+88609123466"));
            return all連絡電話;
        }
    }
}
