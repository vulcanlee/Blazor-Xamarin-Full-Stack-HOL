using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.Helpers;
    using Backend.SortModels;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using ShareBusiness.Factories;
    using ShareBusiness.Helpers;
    using ShareDomain.DataModels;
    using ShareDomain.Enums;

    public class WorkingLogDetailService : IWorkingLogDetailService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public WorkingLogDetailService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<WorkingLogDetailAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<WorkingLogDetailAdapterModel> data = new List<WorkingLogDetailAdapterModel>();
            DataRequestResult<WorkingLogDetailAdapterModel> result = new DataRequestResult<WorkingLogDetailAdapterModel>();
            var DataSource = context.WorkingLogDetail
                .AsNoTracking()
                .Include(x => x.Project)
                .AsQueryable();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Title.Contains(dataRequest.Search) ||
                x.Summary.Contains(dataRequest.Search) ||
                x.Project.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)WorkingLogDetailSortEnum.TitleDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Title);
                        break;
                    case (int)WorkingLogDetailSortEnum.TitleAscending:
                        DataSource = DataSource.OrderBy(x => x.Title);
                        break;
                    case (int)WorkingLogDetailSortEnum.HoursDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Hours);
                        break;
                    case (int)WorkingLogDetailSortEnum.HoursAscending:
                        DataSource = DataSource.OrderBy(x => x.Hours);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<WorkingLogDetail>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<WorkingLogDetailAdapterModel> adapterModelObjects =
                Mapper.Map<List<WorkingLogDetailAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<WorkingLogDetailAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<WorkingLogDetailAdapterModel> data = new List<WorkingLogDetailAdapterModel>();
            DataRequestResult<WorkingLogDetailAdapterModel> result = new DataRequestResult<WorkingLogDetailAdapterModel>();
            var DataSource = context.WorkingLogDetail
                .AsNoTracking()
                .Include(x => x.Project)
                .Where(x => x.WorkingLogId == id);

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<WorkingLogDetail>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<WorkingLogDetailAdapterModel> adapterModelObjects =
                Mapper.Map<List<WorkingLogDetailAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<WorkingLogDetailAdapterModel> GetAsync(int id)
        {
            WorkingLogDetail item = await context.WorkingLogDetail
                .AsNoTracking()
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id);
            WorkingLogDetailAdapterModel result = Mapper.Map<WorkingLogDetailAdapterModel>(item);
            await OhterDependencyData(result); return result;
        }

        public async Task<VerifyRecordResult> AddAsync(WorkingLogDetailAdapterModel paraObject)
        {
            WorkingLogDetail itemParameter = Mapper.Map<WorkingLogDetail>(paraObject);
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            await context.WorkingLogDetail
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            var id = itemParameter.WorkingLogId;
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            await CountingWorkingHour(id);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(WorkingLogDetailAdapterModel paraObject)
        {
            WorkingLogDetail itemData = Mapper.Map<WorkingLogDetail>(paraObject);
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            WorkingLogDetail item = await context.WorkingLogDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<WorkingLogDetail>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                var id = itemData.WorkingLogId;
                CleanTrackingHelper.Clean<WorkingLogDetail>(context);
                await CountingWorkingHour(id);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            WorkingLogDetail item = await context.WorkingLogDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<WorkingLogDetail>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                var pid = item.WorkingLogId;
                CleanTrackingHelper.Clean<WorkingLogDetail>(context);
                await CountingWorkingHour(pid);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> BeforeAddCheckAsync(WorkingLogDetailAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            if (paraObject.ProjectId == 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.尚未輸入該工作日誌的歸屬專案);
            }
            if (paraObject.Hours <= 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.工作時數必須大於0);
            }
            var item = await context.WorkingLogDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WorkingLogId == paraObject.WorkingLogId &&
                x.ProjectId == paraObject.ProjectId);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(WorkingLogDetailAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            if (paraObject.ProjectId == 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.尚未輸入該工作日誌的歸屬專案);
            }
            if (paraObject.Hours <= 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.工作時數必須大於0);
            }
            var item = await context.WorkingLogDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WorkingLogId == paraObject.WorkingLogId &&
                x.ProjectId == paraObject.ProjectId &&
                x.Id != paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public Task<VerifyRecordResult> BeforeDeleteCheckAsync(WorkingLogDetailAdapterModel paraObject)
        {
            return Task.FromResult(VerifyRecordResultFactory.Build(true));
        }

        async Task CountingWorkingHour(int WorkingLogId)
        {
            CleanTrackingHelper.Clean<WorkingLog>(context);
            var totalHours = await context.WorkingLogDetail
                .AsNoTracking()
                .Where(x => x.WorkingLogId == WorkingLogId)
                .SumAsync(x => x.Hours);
            var item = await context.WorkingLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == WorkingLogId);
            if (item != null)
            {
                item.TotalHours = totalHours;
                context.Update(item);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<WorkingLog>(context);
            }
        }
        Task OhterDependencyData(WorkingLogDetailAdapterModel data)
        {
            data.ProjectName = data.Project.Name;
            return Task.FromResult(0);
        }
    }
}
