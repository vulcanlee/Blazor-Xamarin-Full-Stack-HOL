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

    public class MyUserService : IMyUserService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public MyUserService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<MyUserAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<MyUserAdapterModel> data = new List<MyUserAdapterModel>();
            DataRequestResult<MyUserAdapterModel> result = new DataRequestResult<MyUserAdapterModel>();
            var DataSource = context.MyUser
                .AsNoTracking();

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search) ||
                x.Account.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)MyUserSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)MyUserSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)MyUserSortEnum.AccountDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Account);
                        break;
                    case (int)MyUserSortEnum.AccountAscending:
                        DataSource = DataSource.OrderBy(x => x.Account);
                        break;
                    case (int)MyUserSortEnum.DepartmentNameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.DepartmentName);
                        break;
                    case (int)MyUserSortEnum.DepartmentNameAscending:
                        DataSource = DataSource.OrderBy(x => x.DepartmentName);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<MyUser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MyUserAdapterModel> adapterModelObjects =
                Mapper.Map<List<MyUserAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<MyUserAdapterModel> GetAsync(int id)

        {
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                MyUserAdapterModel result = Mapper.Map<MyUserAdapterModel>(item);
                await OhterDependencyData(result);
                return result;
            }
            else
            {
                return new MyUserAdapterModel();
            }
        }

        public async Task<VerifyRecordResult> AddAsync(MyUserAdapterModel paraObject)
        {
            MyUser itemParameter = Mapper.Map<MyUser>(paraObject);
            CleanTrackingHelper.Clean<MyUser>(context);
            await context.MyUser
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MyUser>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(MyUserAdapterModel paraObject)
        {
            MyUser itemData = Mapper.Map<MyUser>(paraObject);
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<MyUser>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MyUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<MyUser>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MyUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserAdapterModel paraObject)
        {
            var searchItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserAdapterModel paraObject)
        {
            var searchItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<WorkingLog>(context);
            CleanTrackingHelper.Clean<LeaveForm>(context);
            CleanTrackingHelper.Clean<ExceptionRecord>(context);
            WorkingLog item = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MyUserId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            LeaveForm itemLeaveForm = await context.LeaveForm
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MyUserId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            ExceptionRecord itemExceptionRecord = await context.ExceptionRecord
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MyUserId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<(MyUserAdapterModel, string)> CheckUser(string account, string password)
        {
            MyUser user = await context.MyUser.AsNoTracking().FirstOrDefaultAsync(x => x.Account == account);
            if (user == null)
            {
                return (null, ErrorMessageMappingHelper.Instance
                    .GetErrorMessage(ErrorMessageEnum.使用者帳號不存在));
            }

            if (user.Password != password)
            {
                return (null, ErrorMessageMappingHelper.Instance
                    .GetErrorMessage(ErrorMessageEnum.密碼不正確));
            }
            MyUserAdapterModel userAdapterModel = Mapper.Map<MyUserAdapterModel>(user);
            return (userAdapterModel, "");
        }

        async Task OhterDependencyData(MyUserAdapterModel data)
        {
            if (data.IsManager == true)
            {
                data.IsManagerString = "是";
            }
            else
            {
                data.IsManagerString = "否";
            }

            var user = await context.MyUser
                .FirstOrDefaultAsync(x => x.Id == data.ManagerId);
            if (user != null)
            {
                data.ManagerName = user.Name;
            }
        }
    }
}
