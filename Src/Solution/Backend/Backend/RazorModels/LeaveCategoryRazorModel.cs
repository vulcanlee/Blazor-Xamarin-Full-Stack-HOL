using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.RazorModels
{
    using AutoMapper;
    using Entities.Models;
    using Backend.AdapterModels;
    using Backend.Interfaces;
    using Backend.Services;
    using Backend.Helpers;
    using Backend.SortModels;
    using Syncfusion.Blazor.Grids;
    using Microsoft.AspNetCore.Components.Forms;
    using ShareDomain.DataModels;
    using ShareBusiness.Helpers;
    using Syncfusion.Blazor.Navigations;

    public class LeaveCategoryRazorModel
    {
        #region Constructor
        public LeaveCategoryRazorModel(ILeaveCategoryService CurrentService,
           BackendDBContext context,
           IMapper Mapper)
        {
            this.CurrentService = CurrentService;
            this.context = context;
            mapper = Mapper;
            LeaveCategorySort.Initialization(SortConditions);

            Toolbaritems.Add(new ItemModel()
            {
                Id = ButtonIdHelper.ButtonIdAdd,
                Text = "新增",
                TooltipText = "新增",
                Type = ItemType.Button,
                PrefixIcon = "mdi mdi-plus-thick",
                Align = ItemAlign.Left,
            });
            Toolbaritems.Add(new ItemModel()
            {
                Id = ButtonIdHelper.ButtonIdRefresh,
                Text = "重新整理",
                TooltipText = "重新整理",
                PrefixIcon = "mdi mdi-refresh",
                Align = ItemAlign.Left,
            });
            Toolbaritems.Add("Search");
        }
        #endregion

        #region Property
        public bool IsShowEditRecord { get; set; } = false;
        public LeaveCategoryAdapterModel CurrentRecord { get; set; } = new LeaveCategoryAdapterModel();
        public LeaveCategoryAdapterModel CurrentNeedDeleteRecord { get; set; } = new LeaveCategoryAdapterModel();
        public EditContext LocalEditContext { get; set; }
        public List<SortCondition> SortConditions { get; set; } = new List<SortCondition>();
        public SortCondition CurrentSortCondition { get; set; } = new SortCondition();

        #region 訊息說明之對話窗使用的變數
        public ConfirmBoxModel ConfirmMessageBox { get; set; } = new ConfirmBoxModel();
        public MessageBoxModel MessageBox { get; set; } = new MessageBoxModel();
        #endregion

        public string EditRecordDialogTitle { get; set; } = "";
        #endregion

        #region Field
        public bool ShowAontherRecordPicker { get; set; } = false;
        bool isNewRecordMode;
        private readonly ILeaveCategoryService CurrentService;
        private readonly BackendDBContext context;
        private readonly IMapper mapper;
        IRazorPage thisRazorComponent;
        IDataGrid dataGrid;
        private bool isShowConfirm { get; set; } = false;
        public List<object> Toolbaritems = new List<object>();
        #endregion

        #region Method

        #region DataGrid 初始化
        public void Setup(IRazorPage razorPage, IDataGrid dataGrid)
        {
            thisRazorComponent = razorPage;
            this.dataGrid = dataGrid;
        }
        #endregion

        #region 工具列事件 (新增)
        public void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == ButtonIdHelper.ButtonIdAdd)
            {
                CurrentRecord = new LeaveCategoryAdapterModel();
                #region 針對新增的紀錄所要做的初始值設定商業邏輯
                #endregion
                EditRecordDialogTitle = "新增紀錄";
                isNewRecordMode = true;
                IsShowEditRecord = true;
            }
            else if (args.Item.Id == ButtonIdHelper.ButtonIdRefresh)
            {
                dataGrid.RefreshGrid();
            }
        }
        #endregion

        #region 記錄列的按鈕事件 (修改與刪除)
        public async Task OnCommandClicked(CommandClickEventArgs<LeaveCategoryAdapterModel> args)
        {
            LeaveCategoryAdapterModel item = args.RowData as LeaveCategoryAdapterModel;
            if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdEdit)
            {
                CurrentRecord = item.Clone();
                EditRecordDialogTitle = "修改紀錄";
                IsShowEditRecord = true;
                isNewRecordMode = false;

            }
            else if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdDelete)
            {
                CurrentNeedDeleteRecord = item;

                #region 檢查關聯資料是否存在
                var checkedResult = await CurrentService
                    .BeforeDeleteCheckAsync(CurrentNeedDeleteRecord);
                if (checkedResult.Success == false)
                {
                    MessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId));
                    return;
                }
                #endregion

                ConfirmMessageBox.Show("400px", "200px", "警告", "確認要刪除這筆紀錄嗎？");
            }
        }

        public async Task RemoveThisRecord(bool NeedDelete)
        {
            if (NeedDelete == true)
            {
                await CurrentService.DeleteAsync(CurrentNeedDeleteRecord.Id);
                dataGrid.RefreshGrid();
            }
            ConfirmMessageBox.Hidden();
        }
        #endregion

        #region 修改紀錄對話窗的按鈕事件
        public void OnEditContestChanged(EditContext context)
        {
            LocalEditContext = context;
        }

        public void OnRecordEditCancel()
        {
            IsShowEditRecord = false;
        }

        public async Task OnRecordEditConfirm()
        {
            #region 進行 Form Validation 檢查驗證作業
            if (LocalEditContext.Validate() == false)
            {
                return;
            }
            #endregion

            #region 檢查資料完整性
            if (isNewRecordMode == true)
            {
                var checkedResult = await CurrentService
                    .BeforeAddCheckAsync(CurrentRecord);
                if (checkedResult.Success == false)
                {
                    ConfirmMessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId));
                    return;
                }
            }
            else
            {
                var checkedResult = await CurrentService
                    .BeforeUpdateCheckAsync(CurrentRecord);
                if (checkedResult.Success == false)
                {
                    ConfirmMessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId));
                    return;
                }
            }
            #endregion

            if (IsShowEditRecord == true)
            {
                if (isNewRecordMode == true)
                {
                    await CurrentService.AddAsync(CurrentRecord);
                    dataGrid.RefreshGrid();
                }
                else
                {
                    await CurrentService.UpdateAsync(CurrentRecord);
                    dataGrid.RefreshGrid();
                }
                IsShowEditRecord = false;
            }
        }
        #endregion

        #region 排序搜尋事件
        public int DefaultSorting { get; set; } = -1;
        public void SortChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, SortCondition> args)
        {
            if (dataGrid.GridIsExist() == true)
            {
                CurrentSortCondition.Id = args.Value;
                dataGrid.RefreshGrid();
            }
        }
        #endregion
        #endregion
    }
}
