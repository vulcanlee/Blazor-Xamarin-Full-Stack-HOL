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

    public class WorkingLogService : IWorkingLogService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public WorkingLogService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<WorkingLogAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<WorkingLogAdapterModel> data = new List<WorkingLogAdapterModel>();
            DataRequestResult<WorkingLogAdapterModel> result = new DataRequestResult<WorkingLogAdapterModel>();
            var DataSource = context.WorkingLog
                .AsNoTracking()
                .Include(x => x.MyUser)
                .AsQueryable();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search) ||
                x.MyUser.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)WorkingLogSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)WorkingLogSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<WorkingLog>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<WorkingLogAdapterModel> adapterModelObjects =
                Mapper.Map<List<WorkingLogAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                adapterModelItem.MyUserName = adapterModelItem.MyUser.Name;
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<WorkingLogAdapterModel> GetAsync(int id)

        {
            WorkingLog item = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            WorkingLogAdapterModel result = Mapper.Map<WorkingLogAdapterModel>(item);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(WorkingLogAdapterModel paraObject)
        {
            WorkingLog itemParameter = Mapper.Map<WorkingLog>(paraObject);
            CleanTrackingHelper.Clean<WorkingLog>(context);
            await context.WorkingLog
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<WorkingLog>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(WorkingLogAdapterModel paraObject)
        {
            WorkingLog itemData = Mapper.Map<WorkingLog>(paraObject);
            CleanTrackingHelper.Clean<WorkingLog>(context);
            WorkingLog item = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<WorkingLog>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<WorkingLog>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<WorkingLog>(context);
            WorkingLog item = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<WorkingLog>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<WorkingLog>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(WorkingLogAdapterModel paraObject)
        {
            var searchItem = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
                x.MyUserId == paraObject.MyUserId);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(WorkingLogAdapterModel paraObject)
        {
            var searchItem = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => (x.Name == paraObject.Name &&
                x.MyUserId == paraObject.MyUserId) &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(WorkingLogAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            WorkingLogDetail item = await context.WorkingLogDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WorkingLogId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
    }
}
