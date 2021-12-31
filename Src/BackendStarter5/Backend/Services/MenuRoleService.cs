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

    public class MenuRoleService : IMenuRoleService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<MenuRoleService> Logger { get; }
        #endregion

        #region 建構式
        public MenuRoleService(BackendDBContext context, IMapper mapper,
            ILogger<MenuRoleService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<MenuRoleAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<MenuRoleAdapterModel> data = new();
            DataRequestResult<MenuRoleAdapterModel> result = new();
            var DataSource = context.MenuRole
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                    .Include(x => x.MenuData)
                    .Where(x => x.Name.Contains(dataRequest.Search) ||
                    x.MenuData.Any(x => x.Name.Contains(dataRequest.Search)));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)MenuRoleSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)MenuRoleSortEnum.NameAscending:
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
            result.Count = DataSource.Cast<MenuRole>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MenuRoleAdapterModel> adapterModelObjects =
                Mapper.Map<List<MenuRoleAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<MenuRoleAdapterModel> GetAsync(int id)
        {
            MenuRole item = await context.MenuRole
                .AsNoTracking()
                .Include(x => x.MenuData)
                .FirstOrDefaultAsync(x => x.Id == id);
            MenuRoleAdapterModel result = Mapper.Map<MenuRoleAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<MenuRoleAdapterModel> GetAsync(string name)
        {
            MenuRole item = await context.MenuRole
                .AsNoTracking()
                .Include(x => x.MenuData)
                .FirstOrDefaultAsync(x => x.Name == name);
            MenuRoleAdapterModel result = Mapper.Map<MenuRoleAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(MenuRoleAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<MenuRole>(context);
                MenuRole itemParameter = Mapper.Map<MenuRole>(paraObject);
                CleanTrackingHelper.Clean<MenuRole>(context);
                await context.MenuRole
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuRole>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(MenuRoleAdapterModel paraObject)
        {
            try
            {
                MenuRole itemData = Mapper.Map<MenuRole>(paraObject);
                CleanTrackingHelper.Clean<MenuRole>(context);
                MenuRole item = await context.MenuRole
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<MenuRole>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MenuRole>(context);
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
                CleanTrackingHelper.Clean<MenuRole>(context);
                MenuRole item = await context.MenuRole
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    if(item.Name == MagicHelper.開發者功能表角色 ||
                        item.Name == MagicHelper.系統管理員功能表角色)
                    {
                        return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                    }

                    CleanTrackingHelper.Clean<MenuRole>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MenuRole>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(MenuRoleAdapterModel paraObject)
        {
            var searchItem = await context.MenuRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(MenuRoleAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<MenuRole>(context);
            var searchItem = await context.MenuRole
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.MenuRole
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
               x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(MenuRoleAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<MenuData>(context);

            var searchItem = await context.MenuRole
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
            }

            MenuData item = await context.MenuData
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MenuRoleId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        #endregion

        #region 其他服務方法
        Task OhterDependencyData(MenuRoleAdapterModel data)
        {
            return Task.FromResult(0);
        }

        public async Task<List<MenuRoleAdapterModel>> GetForUserAsync()
        {
            var result = await context.MenuRole
                .AsNoTracking()
                .ToListAsync();

            List<MenuRoleAdapterModel> menuRoleAdapterModels = new();
            foreach (var item in result)
            {
                menuRoleAdapterModels.Add(new MenuRoleAdapterModel
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return menuRoleAdapterModels;
        }
        #endregion
    }
}
