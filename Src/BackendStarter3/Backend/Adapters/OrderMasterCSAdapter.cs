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
    public partial class OrderMasterCSAdapter : DataAdaptor<IOrderMasterService>
    {
#pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __Blazor.Backend.Adapters.OrderMasterCSAdapter.TypeInference.CreateCascadingValue_0(
                __builder, 0, 1, this, 2, (__builder2) =>
                {
                    __builder2.AddContent(3, ChildContent);
                });
        }

        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public SortCondition CurrentSortCondition { get; set; }

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
            DataRequestResult<OrderMasterAdapterModel> adaptorModelObjects = await Service.GetAsync(dataRequest);
            var item = dataManagerRequest.RequiresCounts
                ? new DataResult() { Result = adaptorModelObjects.Result, Count = adaptorModelObjects.Count }
                : (object)adaptorModelObjects.Result;
            await Task.Yield();
            return item;
            #endregion
        }

        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IMapper Mapper { get; set; }
    }
}
namespace __Blazor.Backend.Adapters.OrderMasterCSAdapter
{
    internal static class TypeInference
    {
        public static void CreateCascadingValue_0<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TValue __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
            __builder.OpenComponent<global::Microsoft.AspNetCore.Components.CascadingValue<TValue>>(seq);
            __builder.AddAttribute(__seq0, "Value", __arg0);
            __builder.AddAttribute(__seq1, "ChildContent", __arg1);
            __builder.CloseComponent();
        }
    }
}

