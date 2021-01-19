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

    public class OnCallPhoneService : IOnCallPhoneService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public OnCallPhoneService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<OnCallPhoneAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<OnCallPhoneAdapterModel> data = new List<OnCallPhoneAdapterModel>();
            DataRequestResult<OnCallPhoneAdapterModel> result = new DataRequestResult<OnCallPhoneAdapterModel>();
            var DataSource = context.OnCallPhone
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Title.Contains(dataRequest.Search) ||
                 x.PhoneNumber.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)OnCallPhoneSortEnum.TitleDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Title);
                        break;
                    case (int)OnCallPhoneSortEnum.TitleAscending:
                        DataSource = DataSource.OrderBy(x => x.Title);
                        break;
                    case (int)OnCallPhoneSortEnum.PhoneNumberDescending:
                        DataSource = DataSource.OrderByDescending(x => x.PhoneNumber);
                        break;
                    case (int)OnCallPhoneSortEnum.PhoneNumberAscending:
                        DataSource = DataSource.OrderBy(x => x.PhoneNumber);
                        break;
                    case (int)OnCallPhoneSortEnum.OrderNumberDescending:
                        DataSource = DataSource.OrderByDescending(x => x.OrderNumber);
                        break;
                    case (int)OnCallPhoneSortEnum.OrderNumberAscending:
                        DataSource = DataSource.OrderBy(x => x.OrderNumber);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<OnCallPhone>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<OnCallPhoneAdapterModel> adapterModelObjects =
                Mapper.Map<List<OnCallPhoneAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<OnCallPhoneAdapterModel> GetAsync(int id)

        {
            OnCallPhone item = await context.OnCallPhone
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            OnCallPhoneAdapterModel result = Mapper.Map<OnCallPhoneAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(OnCallPhoneAdapterModel paraObject)
        {
            OnCallPhone itemParameter = Mapper.Map<OnCallPhone>(paraObject);
            CleanTrackingHelper.Clean<OnCallPhone>(context);
            await context.OnCallPhone
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<OnCallPhone>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(OnCallPhoneAdapterModel paraObject)
        {
            OnCallPhone itemData = Mapper.Map<OnCallPhone>(paraObject);
            CleanTrackingHelper.Clean<OnCallPhone>(context);
            OnCallPhone item = await context.OnCallPhone
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<OnCallPhone>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<OnCallPhone>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<OnCallPhone>(context);
            OnCallPhone item = await context.OnCallPhone
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<OnCallPhone>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<OnCallPhone>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(OnCallPhoneAdapterModel paraObject)
        {
            var searchItem = await context.OnCallPhone
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == paraObject.Title);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(OnCallPhoneAdapterModel paraObject)
        {
            var searchItem = await context.OnCallPhone
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == paraObject.Title &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public Task<VerifyRecordResult> BeforeDeleteCheckAsync(OnCallPhoneAdapterModel paraObject)
        {
            return Task.FromResult(VerifyRecordResultFactory.Build(true));
        }
        Task OhterDependencyData(OnCallPhoneAdapterModel data)
        {
            return Task.FromResult(0);
        }
    }
}
