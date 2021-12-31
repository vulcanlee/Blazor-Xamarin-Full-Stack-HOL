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
    using System;
    using System.Linq;

    public class ExceptionRecordService : IExceptionRecordService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public ExceptionRecordService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<ExceptionRecordAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<ExceptionRecordAdapterModel> data = new List<ExceptionRecordAdapterModel>();
            DataRequestResult<ExceptionRecordAdapterModel> result = new DataRequestResult<ExceptionRecordAdapterModel>();
            var DataSource = context.ExceptionRecord
                .AsNoTracking()
                .Include(x => x.MyUser)
                .AsQueryable();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Message.Contains(dataRequest.Search) ||
                x.CallStack.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)ExceptionRecordSortEnum.MessageDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Message);
                        break;
                    case (int)ExceptionRecordSortEnum.MessageAscending:
                        DataSource = DataSource.OrderBy(x => x.Message);
                        break;
                    case (int)ExceptionRecordSortEnum.ExceptionTimeDescending:
                        DataSource = DataSource.OrderByDescending(x => x.ExceptionTime);
                        break;
                    case (int)ExceptionRecordSortEnum.ExceptionTimeAscending:
                        DataSource = DataSource.OrderBy(x => x.ExceptionTime);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<ExceptionRecord>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<ExceptionRecordAdapterModel> adapterModelObjects =
                Mapper.Map<List<ExceptionRecordAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<ExceptionRecordAdapterModel> GetAsync(int id)

        {
            ExceptionRecord item = await context.ExceptionRecord
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return null;
            }
            ExceptionRecordAdapterModel result = Mapper.Map<ExceptionRecordAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(ExceptionRecordAdapterModel paraObject)
        {
            ExceptionRecord itemParameter = Mapper.Map<ExceptionRecord>(paraObject);
            CleanTrackingHelper.Clean<ExceptionRecord>(context);
            try
            {
                await context.ExceptionRecord
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            CleanTrackingHelper.Clean<ExceptionRecord>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(ExceptionRecordAdapterModel paraObject)
        {
            ExceptionRecord itemData = Mapper.Map<ExceptionRecord>(paraObject);
            CleanTrackingHelper.Clean<ExceptionRecord>(context);
            ExceptionRecord item = await context.ExceptionRecord
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<ExceptionRecord>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<ExceptionRecord>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<ExceptionRecord>(context);
            ExceptionRecord item = await context.ExceptionRecord
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<ExceptionRecord>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<ExceptionRecord>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(ExceptionRecordAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(ExceptionRecordAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(ExceptionRecordAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }
        async Task OhterDependencyData(ExceptionRecordAdapterModel data)
        {
            try
            {
                if (data.MyUserId != null)
                {
                    CleanTrackingHelper.Clean<MyUser>(context);
                    var user = await context.MyUser
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == data.MyUserId);
                    if (user != null)
                    {
                        data.MyUserName = user.Name;
                    }
                }
            }
            catch (Exception)
            {
            }
            return;
        }
    }
}
