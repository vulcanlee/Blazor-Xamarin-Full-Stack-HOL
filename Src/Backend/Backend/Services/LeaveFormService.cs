﻿using System;
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

    public class LeaveFormService : ILeaveFormService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public LeaveFormService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<LeaveFormAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<LeaveFormAdapterModel> data = new List<LeaveFormAdapterModel>();
            DataRequestResult<LeaveFormAdapterModel> result = new DataRequestResult<LeaveFormAdapterModel>();
            var DataSource = context.LeaveForm
                .AsNoTracking()
                .Include(x => x.MyUser)
                .Include(x => x.LeaveCategory)
                .AsQueryable();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.MyUser.Name.Contains(dataRequest.Search) ||
                x.LeaveCategory.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)LeaveFormSortEnum.FormDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.FormDate);
                        break;
                    case (int)LeaveFormSortEnum.FormDateAscending:
                        DataSource = DataSource.OrderBy(x => x.FormDate);
                        break;
                    case (int)LeaveFormSortEnum.HoursDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Hours);
                        break;
                    case (int)LeaveFormSortEnum.HoursAscending:
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
            result.Count = DataSource.Cast<LeaveForm>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<LeaveFormAdapterModel> adapterModelObjects =
                Mapper.Map<List<LeaveFormAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<LeaveFormAdapterModel> GetAsync(int id)

        {
            LeaveForm item = await context.LeaveForm
                .AsNoTracking()
                .Include(x => x.MyUser)
                .Include(x => x.LeaveCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                LeaveFormAdapterModel result = Mapper.Map<LeaveFormAdapterModel>(item);
                await OhterDependencyData(result);
                return result;
            }
            else
            {
                return new LeaveFormAdapterModel();
            }
        }

        public async Task<VerifyRecordResult> AddAsync(LeaveFormAdapterModel paraObject)
        {
            LeaveForm itemParameter = Mapper.Map<LeaveForm>(paraObject);
            CleanTrackingHelper.Clean<LeaveForm>(context);
            await context.LeaveForm
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<LeaveForm>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(LeaveFormAdapterModel paraObject)
        {
            LeaveForm itemData = Mapper.Map<LeaveForm>(paraObject);
            CleanTrackingHelper.Clean<LeaveForm>(context);
            LeaveForm item = await context.LeaveForm
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<LeaveForm>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<LeaveForm>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<LeaveForm>(context);
            LeaveForm item = await context.LeaveForm
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<LeaveForm>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<LeaveForm>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(LeaveFormAdapterModel paraObject)
        {
            var searchItem = await context.LeaveForm
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BeginDate == paraObject.BeginDate);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            LeaveCategory itemLeaveCategory = await context.LeaveCategory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.LeaveCategoryId);
            if (itemLeaveCategory == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定請假類別);
            }
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.MyUserId);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定使用者);
            }
            item = await context.MyUser
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == paraObject.AgentId);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定使用者);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(LeaveFormAdapterModel paraObject)
        {
            var searchItem = await context.LeaveForm
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BeginDate == paraObject.BeginDate &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            LeaveCategory itemLeaveCategory = await context.LeaveCategory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.LeaveCategoryId);
            if (itemLeaveCategory == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定請假類別);
            }
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.MyUserId);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定使用者);
            }
            item = await context.MyUser
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == paraObject.AgentId);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定使用者);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(LeaveFormAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }
        async Task OhterDependencyData(LeaveFormAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            var myUser = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.AgentId);
            if (myUser != null)
            {
                data.AgentName = myUser.Name;
            }
            var leaveCategory = await context.LeaveCategory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == data.LeaveCategoryId);
            if (leaveCategory != null)
            {
                data.LeaveCategoryName = leaveCategory.Name;
            }
        }
    }
}
