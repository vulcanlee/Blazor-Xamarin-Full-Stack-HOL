using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.ViewModels
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.Helpers;
    using Backend.Interfaces;
    using Backend.Services;
    using Backend.SortModels;
    using Domains.Models;
    using Microsoft.AspNetCore.Components.Forms;
    using BAL.Helpers;
    using CommonDomain.DataModels;
    using Syncfusion.Blazor.Grids;
    using Syncfusion.Blazor.Navigations;
    using Backend.Models;

    public class SystemLogViewModel
    {
        #region Constructor
        public SystemLogViewModel(ISystemLogService CurrentService,
           BackendDBContext context, IMapper Mapper,
           TranscationResultHelper transcationResultHelper)
        {
            this.CurrentService = CurrentService;
            this.context = context;
            mapper = Mapper;
            TranscationResultHelper = transcationResultHelper;
            SystemLogSort.Initialization(SortConditions);

            #region 工具列按鈕初始化
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
            #endregion
        }
        #endregion

        #region Property
        /// <summary>
        /// 是否要顯示紀錄新增或修改對話窗 
        /// </summary>
        public bool IsShowEditRecord { get; set; } = false;
        /// <summary>
        /// 是否要顯示關聯多筆資料表的 CRUD 對話窗
        /// </summary>
        public bool IsShowMoreDetailsRecord { get; set; } = false;
        /// <summary>
        /// 現在正在新增或修改的紀錄  
        /// </summary>
        public SystemLogAdapterModel CurrentRecord { get; set; } = new SystemLogAdapterModel();
        /// <summary>
        /// 現在正在刪除的紀錄  
        /// </summary>
        public SystemLogAdapterModel CurrentNeedDeleteRecord { get; set; } = new SystemLogAdapterModel();
        /// <summary>
        /// 保存與資料編輯程式相關的中繼資料
        /// </summary>
        public EditContext LocalEditContext { get; set; }
        /// <summary>
        /// 是否顯示選取其他清單記錄對話窗 
        /// </summary>
        public bool ShowAontherRecordPicker { get; set; } = false;
        /// <summary>
        /// 父參考物件的 Id 
        /// </summary>
        public MasterRecord Header { get; set; } = new MasterRecord();
        /// <summary>
        /// 可以選擇排序條件清單
        /// </summary>
        public List<SortCondition> SortConditions { get; set; } = new List<SortCondition>();
        /// <summary>
        /// 現在選擇排序條件項目
        /// </summary>
        public SortCondition CurrentSortCondition { get; set; } = new SortCondition();
        /// <summary>
        /// 用於控制、更新明細清單 Grid 
        /// </summary>
        public IDataGrid ShowMoreDetailsGrid { get; set; }
        /// <summary>
        /// 明細清單 Grid 的對話窗主題 
        /// </summary>
        public string ShowMoreDetailsRecordDialogTitle { get; set; } = "";
        /// <summary>
        /// 新增或修改對話窗的標題 
        /// </summary>
        public string EditRecordDialogTitle { get; set; } = "";
        /// <summary>
        /// 指定 Grid 上方可以使用的按鈕項目清單
        /// </summary>
        public List<object> Toolbaritems { get; set; } = new List<object>();

        #region 訊息說明之對話窗使用的變數
        /// <summary>
        /// 確認對話窗設定
        /// </summary>
        public ConfirmBoxModel ConfirmMessageBox { get; set; } = new ConfirmBoxModel();
        /// <summary>
        /// 訊息對話窗設定
        /// </summary>
        public MessageBoxModel MessageBox { get; set; } = new MessageBoxModel();
        #endregion
        #endregion

        #region Field
        /// <summary>
        /// 對選取紀錄進行 新增 或者 修改 
        /// </summary>
        bool isNewRecordMode;
        /// <summary>
        /// 當前記錄需要用到的 Service 物件 
        /// </summary>
        private readonly ISystemLogService CurrentService;
        private readonly BackendDBContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// 這個元件整體的通用介面方法
        /// </summary>
        IRazorPage thisView;
        /// <summary>
        /// 當前 Grid 元件可以使用的通用方法
        /// </summary>
        IDataGrid dataGrid;
        #endregion

        #region Method

        #region DataGrid 初始化
        /// <summary>
        /// 將會於 生命週期事件 OnInitialized / OnAfterRenderAsync 觸發此方法
        /// </summary>
        /// <param name="razorPage">當前元件的物件</param>
        /// <param name="dataGrid">當前 Grid 的元件</param>
        public void Setup(IRazorPage razorPage, IDataGrid dataGrid)
        {
            thisView = razorPage;
            this.dataGrid = dataGrid;
        }
        #endregion

        #region 工具列事件 (新增)
        public void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == ButtonIdHelper.ButtonIdAdd)
            {
                CurrentRecord = new SystemLogAdapterModel();
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
        public async Task OnCommandClicked(CommandClickEventArgs<SystemLogAdapterModel> args)
        {
            SystemLogAdapterModel item = args.RowData as SystemLogAdapterModel;
            if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdRead)
            {
                #region 點選 修改紀錄 按鈕
                CurrentRecord = item.Clone();
                EditRecordDialogTitle = "修改紀錄";
                IsShowEditRecord = true;
                isNewRecordMode = false;
                #endregion
            }
            else if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdDelete)
            {
                #region 點選 刪除紀錄 按鈕
                CurrentNeedDeleteRecord = item;

                #region 檢查關聯資料是否存在
                var checkedResult = await CurrentService
                    .BeforeDeleteCheckAsync(CurrentNeedDeleteRecord);
                await Task.Delay(100);
                if (checkedResult.Success == false)
                {
                    MessageBox.Show("400px", "200px", "警告",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId),
                        MessageBox.HiddenAsync);
                    await Task.Yield();
                    await thisView.NeedRefreshAsync();
                    return;
                }
                #endregion

                #region 刪除這筆紀錄
                await Task.Yield();
                var checkTask = ConfirmMessageBox.ShowAsync("400px", "200px", "警告",
                     "確認要刪除這筆紀錄嗎?", ConfirmMessageBox.HiddenAsync);
                await thisView.NeedRefreshAsync();
                var checkAgain = await checkTask;
                if (checkAgain == true)
                {
                    var verifyRecordResult = await CurrentService.DeleteAsync(CurrentNeedDeleteRecord.Id);
                    await TranscationResultHelper.CheckDatabaseResult(MessageBox, verifyRecordResult);
                    dataGrid.RefreshGrid();
                }
                #endregion
                #endregion
            }
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
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId),
                        MessageBox.HiddenAsync);
                    await thisView.NeedRefreshAsync();
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
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId),
                        MessageBox.HiddenAsync);
                    await thisView.NeedRefreshAsync();
                    return;
                }
            }
            #endregion

            if (IsShowEditRecord == true)
            {
                if (isNewRecordMode == true)
                {
                    var verifyRecordResult = await CurrentService.AddAsync(CurrentRecord);
                    await TranscationResultHelper.CheckDatabaseResult(MessageBox, verifyRecordResult);
                    dataGrid.RefreshGrid();
                }
                else
                {
                    var verifyRecordResult = await CurrentService.UpdateAsync(CurrentRecord);
                    await TranscationResultHelper.CheckDatabaseResult(MessageBox, verifyRecordResult);
                    dataGrid.RefreshGrid();
                }
                IsShowEditRecord = false;
            }
        }
        #endregion

        #region 開窗選取紀錄使用到的方法
        //public void OnOpenPicker()
        //{
        //    ShowAontherRecordPicker = true;
        //}

        //public void OnPickerCompletion(MyUserAdapterModel e)
        //{
        //    if (e != null)
        //    {
        //        CurrentRecord.Id = e.Id;
        //        CurrentRecord.Name = e.Name;
        //    }
        //    ShowAontherRecordPicker = false;
        //}
        #endregion

        #region 排序搜尋事件
        public int DefaultSorting { get; set; } = -1;
        public TranscationResultHelper TranscationResultHelper { get; }

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
