namespace Backend.Adapters
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using AutoMapper;
    using Backend.Services;
    using Backend.AdapterModels;
    using CommonDomain.DataModels;
    using Newtonsoft.Json;
    using Syncfusion.Blazor;
    using Syncfusion.Blazor.Data;
    using Backend.Models;

    public partial class MailQueueAdapter : DataAdaptor<IMailQueueService>
    {
        [Parameter]
        public SortCondition CurrentSortCondition { get; set; }
        [Parameter]
        public MailQueueStatusCondition CurrentMailQueueStatusCondition { get; set; }

        public override async Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null)
        {
            #region 建立查詢物件
            DataRequest dataRequest = new DataRequest()
            {
                Skip = dataManagerRequest.Skip,
                Take = dataManagerRequest.Take,
            };
            if (dataManagerRequest.Search != null && dataManagerRequest.Search.Count > 0)
            {
                var keyword = dataManagerRequest.Search[0].Key;
                dataRequest.Search = keyword;
            }
            if (CurrentSortCondition != null)
            {
                dataRequest.Sorted = CurrentSortCondition;
            }
            #endregion

            #region 發出查詢要求
            try
            {
                DataRequestResult<MailQueueAdapterModel> adaptorModelObjects = await Service.GetAsync(dataRequest,
                    CurrentMailQueueStatusCondition);
                var item = dataManagerRequest.RequiresCounts
                    ? new DataResult() { Result = adaptorModelObjects.Result, Count = adaptorModelObjects.Count }
                    : (object)adaptorModelObjects.Result;
                await Task.Yield();
                return item;
            }
            catch (Exception)
            {
                return new DataResult() { Result = new List<MailQueueAdapterModel>(), Count = 0 };
            }
            #endregion
        }
    }
}

