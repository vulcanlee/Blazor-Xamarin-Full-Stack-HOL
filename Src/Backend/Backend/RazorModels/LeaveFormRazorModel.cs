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

    public class LeaveFormRazorModel
    {
        #region Constructor
        public LeaveFormRazorModel(ILeaveFormService CurrentService,
            ILeaveCategoryService leaveCategoryService,
           BackendDBContext context,
           IMapper Mapper)
        {
            this.CurrentService = CurrentService;
            this.leaveCategoryService = leaveCategoryService;
            this.context = context;
            mapper = Mapper;
            LeaveFormSort.Initialization(SortConditions);

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
        public LeaveFormAdapterModel CurrentRecord { get; set; } = new LeaveFormAdapterModel();
        public LeaveFormAdapterModel CurrentNeedDeleteRecord { get; set; } = new LeaveFormAdapterModel();
        public EditContext LocalEditContext { get; set; }
        public List<SortCondition> SortConditions { get; set; } = new List<SortCondition>();
        public SortCondition CurrentSortCondition { get; set; } = new SortCondition();
        public List<LeaveCategoryAdapterModel> LeaveCategory = new List<LeaveCategoryAdapterModel>();
        public string UserMode { get; set; }
        #region 訊息說明之對話窗使用的變數
        public ConfirmBoxModel ConfirmMessageBox { get; set; } = new ConfirmBoxModel();
        public MessageBoxModel MessageBox { get; set; } = new MessageBoxModel();
        #endregion

        public string EditRecordDialogTitle { get; set; } = "";
        #endregion

        #region Field
        public bool ShowAontherRecordPicker { get; set; } = false;
        bool isNewRecordMode;
        private readonly ILeaveFormService CurrentService;
        private readonly ILeaveCategoryService leaveCategoryService;
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
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == ButtonIdHelper.ButtonIdAdd)
            {
                CurrentRecord = new LeaveFormAdapterModel();
                #region 針對新增的紀錄所要做的初始值設定商業邏輯
                CurrentRecord.FormDate = DateTime.Now;
                CurrentRecord.BeginDate = DateTime.Now;
                CurrentRecord.CompleteDate = DateTime.Now;
                await 取得額外清單查詢物件Async();
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

        private async Task 取得額外清單查詢物件Async()
        {
            #region 取得額外清單查詢物件
            DataRequest dataRequest = new DataRequest()
            {
                Skip = 0,
                Take = 0,
                Search = "",
                Sorted = null,
            };
            var records = await leaveCategoryService.GetAsync(dataRequest);
            LeaveCategory.AddRange(records.Result);
            #endregion
        }
        #endregion

        #region 記錄列的按鈕事件 (修改與刪除)
        public async Task OnCommandClicked(CommandClickEventArgs<LeaveFormAdapterModel> args)
        {
            LeaveFormAdapterModel item = args.RowData as LeaveFormAdapterModel;
            if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdEdit)
            {
                CurrentRecord = item.Clone();

                await 取得額外清單查詢物件Async();

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
                await Task.Delay(100);
                if (checkedResult.Success == false)
                {
                    MessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId));
                    await Task.Yield();
                    thisRazorComponent.NeedRefresh();
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
                    MessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId));
                    thisRazorComponent.NeedRefresh();
                    return;
                }
            }
            else
            {
                var checkedResult = await CurrentService
                    .BeforeUpdateCheckAsync(CurrentRecord);
                if (checkedResult.Success == false)
                {
                    MessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId));
                    thisRazorComponent.NeedRefresh();
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

        #region 開窗選取紀錄使用到的方法
        public void OnOpenPicker(string userMode)
        {
            ShowAontherRecordPicker = true;
            UserMode = userMode;
        }

        public void OnPickerCompletion(MyUserAdapterModel e)
        {
            if (e != null)
            {
                if (UserMode == MagicHelper.MyUserUserMode)
                {
                    CurrentRecord.MyUserId = e.Id;
                    CurrentRecord.MyUserName = e.Name;
                }
                else
                {
                    CurrentRecord.AgentId = e.Id;
                    CurrentRecord.AgentName = e.Name;
                }
            }
            ShowAontherRecordPicker = false;
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
