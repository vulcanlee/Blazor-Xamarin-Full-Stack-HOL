using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using ShareBusiness.Helpers;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;
    using ShareDomain.DataModels;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using ShareBusiness.Factories;
    using ShareDomain.Enums;

    public class HoluserService : IHoluserService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public HoluserService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<HoluserAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<Holuser> data = new List<Holuser>();
            DataRequestResult<HoluserAdapterModel> result = new DataRequestResult<HoluserAdapterModel>();
            var DataSource = context.Holusers
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
                    case (int)HoluserSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)HoluserSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)HoluserSortEnum.AccountDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Account);
                        break;
                    case (int)HoluserSortEnum.AccountAscending:
                        DataSource = DataSource.OrderBy(x => x.Account);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<Holuser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<HoluserAdapterModel> adaptorModelObjects =
                Mapper.Map<List<HoluserAdapterModel>>(DataSource);

            foreach (var adaptorModelItem in adaptorModelObjects)
            {
                // ??? 這裡需要完成管理者人員的相關資料讀取程式碼
            }
            #endregion

            result.Result = adaptorModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<HoluserAdapterModel> GetAsync(int id)
        {
            Holuser item = await context.Holusers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            HoluserAdapterModel result = Mapper.Map<HoluserAdapterModel>(item);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(HoluserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<Holuser>(context);
            Holuser itemParameter = Mapper.Map<Holuser>(paraObject);
            CleanTrackingHelper.Clean<Holuser>(context);
            await context.Holusers
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<Holuser>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(HoluserAdapterModel paraObject)
        {
            Holuser itemData = Mapper.Map<Holuser>(paraObject);
            CleanTrackingHelper.Clean<Holuser>(context);
            Holuser item = await context.Holusers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Holuser>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Holuser>(context);
                return VerifyRecordResultFactory.Build(true);
            }

        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<Holuser>(context);
            Holuser item = await context.Holusers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Holuser>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Holuser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<(HoluserAdapterModel, string)> CheckUser(string account, string password)
        {
            Holuser user = await context.Holusers.AsNoTracking().FirstOrDefaultAsync(x => x.Account == account);
            if(user==null)
            {
                return (null, "使用者帳號不存在");
            }

            if(user.Password != password)
            {
                return (null, "密碼不正確");
            }
            HoluserAdapterModel userAdapterModel = Mapper.Map<HoluserAdapterModel>(user);
            return (userAdapterModel, "");
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(HoluserAdapterModel paraObject)
        {
            var searchItem = await context.Holusers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account);
            if(searchItem !=null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(HoluserAdapterModel paraObject)
        {
            var searchItem = await context.Holusers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public Task<VerifyRecordResult> BeforeDeleteCheckAsync(HoluserAdapterModel paraObject)
        {
            return Task.FromResult(VerifyRecordResultFactory.Build(true));
        }
    }
}
