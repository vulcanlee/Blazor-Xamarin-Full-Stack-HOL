using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using Domains.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using BAL.Factories;
    using BAL.Helpers;
    using CommonDomain.DataModels;
    using CommonDomain.Enums;

    public class MenuDataService : IMenuDataService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<MenuDataService> Logger { get; }
        #endregion

        #region 建構式
        public MenuDataService(BackendDBContext context, IMapper mapper,
            ILogger<MenuDataService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<MenuDataAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<MenuDataAdapterModel> data = new();
            DataRequestResult<MenuDataAdapterModel> result = new();
            var DataSource = context.MenuData
                .AsNoTracking()
                .Include(x => x.MenuRole)
                .AsQueryable();
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
                    case (int)MenuDataSortEnum.Default:
                        DataSource = DataSource.OrderBy(x => x.Sequence);
                        break;
                    case (int)MenuDataSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)MenuDataSortEnum.NameAscending:
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
            result.Count = DataSource.Cast<MenuData>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MenuDataAdapterModel> adapterModelObjects =
                Mapper.Map<List<MenuDataAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<MenuDataAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<MenuDataAdapterModel> data = new();
            DataRequestResult<MenuDataAdapterModel> result = new();
            var DataSource = context.MenuData
                .AsNoTracking()
                .Include(x => x.MenuRole)
                .Where(x => x.MenuRoleId == id);
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
                    case (int)MenuDataSortEnum.Default:
                        DataSource = DataSource.OrderBy(x => x.Sequence);
                        break;
                    case (int)MenuDataSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)MenuDataSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<MenuData>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MenuDataAdapterModel> adapterModelObjects =
                Mapper.Map<List<MenuDataAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<MenuDataAdapterModel> GetAsync(int id)
        {
            MenuData item = await context.MenuData
                .AsNoTracking()
                .Include(x => x.MenuRole)
                .FirstOrDefaultAsync(x => x.Id == id);
            MenuDataAdapterModel result = Mapper.Map<MenuDataAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(MenuDataAdapterModel paraObject)
        {
            try
            {
                MenuData itemParameter = Mapper.Map<MenuData>(paraObject);
                CleanTrackingHelper.Clean<MenuData>(context);
                await context.MenuData
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(MenuDataAdapterModel paraObject)
        {
            try
            {
                MenuData itemData = Mapper.Map<MenuData>(paraObject);
                CleanTrackingHelper.Clean<MenuData>(context);
                MenuData item = await context.MenuData
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<MenuData>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MenuData>(context);
                    return VerifyRecordResultFactory.Build(true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "修改記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "修改記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            try
            {
                CleanTrackingHelper.Clean<MenuData>(context);
                MenuData item = await context.MenuData
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<MenuData>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MenuData>(context);
                    return VerifyRecordResultFactory.Build(true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "刪除記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "刪除記錄發生例外異常", ex);
            }
        }
        #endregion

        #region CRUD 的限制條件檢查
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(MenuDataAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<MenuData>(context);
            if (paraObject.MenuRoleId == 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.尚未輸入功能表的角色);
            }
            var item = await context.MenuData
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MenuRoleId == paraObject.MenuRoleId &&
                x.Name == paraObject.Name);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該功能項目已經存在該功能表的角色);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(MenuDataAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<MenuData>(context);
            var searchItem = await context.MenuData
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            if (paraObject.MenuRoleId == 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.尚未輸入功能表的角色);
            }
            var item = await context.MenuData
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MenuRoleId == paraObject.MenuRoleId &&
                x.Name == paraObject.Name &&
                x.Id != paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該功能項目已經存在該功能表的角色);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(MenuDataAdapterModel paraObject)
        {
            var searchItem = await context.MenuData
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        #endregion

        #region 其他服務方法
        Task OhterDependencyData(MenuDataAdapterModel data)
        {
            if (data.IsGroup == true)
            {
                data.IsGroupName = "是";
            }
            else
            {
                data.IsGroupName = "否";
            }
            if (data.Enable == true)
            {
                data.EnableName = "是";
            }
            else
            {
                data.EnableName = "否";
            }
            return Task.FromResult(0);
        }

        public async Task ReorderByHeaderIDAsync(int id)
        {
            var DataSource = context.MenuData
                .Include(x => x.MenuRole)
                .OrderBy(x => x.Sequence)
                .Where(x => x.MenuRoleId == id);

            var allMenuData = await DataSource.ToListAsync();
            int orderid = 10;
            foreach (var item in allMenuData)
            {
                item.Sequence = orderid;
                orderid += 10;
            }
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MenuData>(context);
            return;
        }
        #endregion

        #region 紀錄啟用或停用
        public async Task DisableIt(MenuDataAdapterModel paraObject)
        {
            MenuData itemData = Mapper.Map<MenuData>(paraObject);
            CleanTrackingHelper.Clean<MenuData>(context);
            MenuData item = await context.MenuData
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Enable = false;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }

        public async Task EnableIt(MenuDataAdapterModel paraObject)
        {
            MenuData itemData = Mapper.Map<MenuData>(paraObject);
            CleanTrackingHelper.Clean<MenuData>(context);
            MenuData item = await context.MenuData
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Enable = true;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }
        #endregion

    }
}
