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
            var items = Get假別();
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
                }
            }
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<LeaveCategory>(context);

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
                }
            }
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<OnCallPhone>(context);
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
