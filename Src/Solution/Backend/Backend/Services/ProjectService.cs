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

    public class ProjectService : IProjectService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public ProjectService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<ProjectAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<ProjectAdapterModel> data = new List<ProjectAdapterModel>();
            DataRequestResult<ProjectAdapterModel> result = new DataRequestResult<ProjectAdapterModel>();
            var DataSource = context.Project
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)ProjectSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)ProjectSortEnum.NameAscending:
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
            result.Count = DataSource.Cast<Project>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<ProjectAdapterModel> adaptorModelObjects =
                Mapper.Map<List<ProjectAdapterModel>>(DataSource);

            foreach (var adaptorModelItem in adaptorModelObjects)
            {
                // ??? 這裡需要完成管理者人員的相關資料讀取程式碼
            }
            #endregion

            result.Result = adaptorModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<ProjectAdapterModel> GetAsync(int id)

        {
            Project item = await context.Project
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            ProjectAdapterModel result = Mapper.Map<ProjectAdapterModel>(item);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(ProjectAdapterModel paraObject)
        {
            Project itemParameter = Mapper.Map<Project>(paraObject);
            CleanTrackingHelper.Clean<Project>(context);
            await context.Project
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<Project>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(ProjectAdapterModel paraObject)
        {
            Project itemData = Mapper.Map<Project>(paraObject);
            CleanTrackingHelper.Clean<Project>(context);
            Project item = await context.Project
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Project>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Project>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<Project>(context);
            Project item = await context.Project
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Project>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Project>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(ProjectAdapterModel paraObject)
        {
            var searchItem = await context.Project
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(ProjectAdapterModel paraObject)
        {
            var searchItem = await context.Project
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(ProjectAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<WorkingLogDetail>(context);
            WorkingLogDetail item = await context.WorkingLogDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
    }
}
