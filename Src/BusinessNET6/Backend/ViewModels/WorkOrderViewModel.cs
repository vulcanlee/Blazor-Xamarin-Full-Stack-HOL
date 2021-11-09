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
    using System;
    using Backend.Models;
    using System.Linq;
    using System.Threading;
    using Newtonsoft.Json;

    public class WorkOrderViewModel
    {
        #region Constructor
        public WorkOrderViewModel(IWorkOrderService CurrentService,
           BackendDBContext context, IMapper Mapper,
           TranscationResultHelper transcationResultHelper,
           IFlowMasterService flowMasterService,
           UserHelper currentUserHelper, CurrentUser currentUser,
           ICategoryMainService categoryMainService, ICategorySubService categorySubService)
        {
            this.CurrentService = CurrentService;
            this.context = context;
            mapper = Mapper;
            TranscationResultHelper = transcationResultHelper;
            FlowMasterService = flowMasterService;
            CurrentUserHelper = currentUserHelper;
            CurrentUser = currentUser;
            CategoryMainService = categoryMainService;
            CategorySubService = categorySubService;
            WorkOrderSort.Initialization(SortConditions);
            WorkOrderStatusCondition.Initialization(WorkOrderStatusConditions);
            CurrentWorkOrderStatusCondition.Id = WorkOrderStatusConditions[0].Id;
            CurrentWorkOrderStatusCondition.Title = WorkOrderStatusConditions[0].Title;
            FilterWorkOrderStatusCondition = CurrentWorkOrderStatusCondition.Id;
            WorkOrderStatusCondition.Initialization(WorkOrderStatusRecordConditions);
            WorkOrderStatusRecordConditions.RemoveAt(0);

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

            #region 互動式彈出功能表 ContextMenu 初始化
            ContextMenuItems.Add(new ContextMenuItemModel
            {
                Text = "送審",
                Target = ".e-content",
                Id = "送審",
                IconCss = "mdi mdi-send-circle",
            });
            ContextMenuItems.Add(new ContextMenuItemModel
            {
                Text = "查看送審記錄",
                Target = ".e-content",
                Id = "查看送審記錄",
                IconCss = "mdi mdi-file-find",
            });
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
        public WorkOrderAdapterModel CurrentRecord { get; set; } = new WorkOrderAdapterModel();
        public FlowMasterAdapterModel CurrentFlowMasterAdapterModel { get; set; } = new FlowMasterAdapterModel();
        /// <summary>
        /// 現在正在刪除的紀錄  
        /// </summary>
        public WorkOrderAdapterModel CurrentNeedDeleteRecord { get; set; } = new WorkOrderAdapterModel();
        /// <summary>
        /// 保存與資料編輯程式相關的中繼資料
        /// </summary>
        public EditContext LocalEditContext { get; set; }
        /// <summary>
        /// 是否顯示選取其他清單記錄對話窗 
        /// </summary>
        public bool ShowAontherRecordPicker { get; set; } = false;
        public bool ShowUserPicker { get; set; } = false;
        public bool ShowReviewFlowDialog { get; set; } = false;
        public bool ShowWorkOrderSendingDialog { get; set; } = false;
        /// <summary>
        /// 父參考物件的 Id 
        /// </summary>
        public MasterRecord Header { get; set; } = new MasterRecord();
        /// <summary>
        /// 可以選擇排序條件清單
        /// </summary>
        public List<SortCondition> SortConditions { get; set; } = new List<SortCondition>();
        public List<WorkOrderStatusCondition> WorkOrderStatusConditions { get; set; } = new List<WorkOrderStatusCondition>();
        public List<WorkOrderStatusCondition> WorkOrderStatusRecordConditions { get; set; } = new List<WorkOrderStatusCondition>();
        /// <summary>
        /// 現在選擇排序條件項目
        /// </summary>
        public SortCondition CurrentSortCondition { get; set; } = new SortCondition();
        public SortCondition CurrentCategoryMainCondition { get; set; } = new SortCondition()
        {
            Id = (int)CategoryMainSortEnum.NameAscending
        };
        public SortCondition CurrentCategorySubCondition { get; set; } = new SortCondition()
        {
            Id = (int)CategorySubSortEnum.OrderNumberAscending
        };
        public WorkOrderStatusCondition CurrentWorkOrderStatusCondition { get; set; } = new WorkOrderStatusCondition();
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
        public List<object> ContextMenuItems { get; set; } = new List<object>();
        public int FilterWorkOrderStatusCondition { get; set; }

        public List<CategoryMainAdapterModel> CategoryMainAdapterModels { get; set; } = new List<CategoryMainAdapterModel>();
        public List<CategorySubAdapterModel> CategorySubAdapterModels { get; set; } = new List<CategorySubAdapterModel>();

        #region 訊息說明之對話窗使用的變數
        /// <summary>
        /// 確認對話窗設定
        /// </summary>
        public ConfirmBoxModel ConfirmMessageBox { get; set; } = new ConfirmBoxModel();
        /// <summary>
        /// 訊息對話窗設定
        /// </summary>
        public MessageBoxModel MessageBox { get; set; } = new MessageBoxModel();
        public TranscationResultHelper TranscationResultHelper { get; }
        public IFlowMasterService FlowMasterService { get; }
        public UserHelper CurrentUserHelper { get; }
        public CurrentUser CurrentUser { get; }
        public ICategoryMainService CategoryMainService { get; }
        public ICategorySubService CategorySubService { get; }
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
        private readonly IWorkOrderService CurrentService;
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
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == ButtonIdHelper.ButtonIdAdd)
            {
                CurrentRecord = new WorkOrderAdapterModel();
                #region 針對新增的紀錄所要做的初始值設定商業邏輯
                #endregion
                EditRecordDialogTitle = "新增紀錄";
                isNewRecordMode = true;
                IsShowEditRecord = true;

                CurrentRecord.CreatedAt = DateTime.Now;
                CurrentRecord.Status = 0;
                CurrentRecord.Code = UniqueStringHelper.GetCode();

                await GetCategoryMainAdapterModels();
            }
            else if (args.Item.Id == ButtonIdHelper.ButtonIdRefresh)
            {
                dataGrid.RefreshGrid();
            }
        }
        #endregion

        #region 記錄列的按鈕事件 (修改與刪除與明細紀錄瀏覽)
        public async Task OnCommandClicked(CommandClickEventArgs<WorkOrderAdapterModel> args)
        {
            WorkOrderAdapterModel item = args.RowData as WorkOrderAdapterModel;
            if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdEdit)
            {
                #region 點選 修改紀錄 按鈕
                CurrentRecord = item.Clone();
                EditRecordDialogTitle = "修改紀錄";
                IsShowEditRecord = true;
                isNewRecordMode = false;

                await GetCategoryMainAdapterModels();
                await GetCategorySubAdapterModels(item.CategoryMainId);

                thisView.NeedRefresh();
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
                    thisView.NeedRefresh();
                    return;
                }
                #endregion

                #region 刪除這筆紀錄
                await Task.Yield();
                var checkTask = ConfirmMessageBox.ShowAsync("400px", "200px", "警告",
                     "確認要刪除這筆紀錄嗎?", ConfirmMessageBox.HiddenAsync);
                thisView.NeedRefresh();
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
            else if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdShowDetailOfMaster)
            {
                #region 點選 開啟多筆 CRUD 對話窗 按鈕
                IsShowMoreDetailsRecord = true;
                ShowMoreDetailsRecordDialogTitle = MagicHelper.訂單明細管理功能名稱;
                MasterRecord masterRecord = new MasterRecord()
                {
                    Id = item.Id
                };
                Header = masterRecord;
                if (ShowMoreDetailsGrid != null)
                {
                    await Task.Delay(100); // 使用延遲，讓 Header 的資料綁定可以成功
                    ShowMoreDetailsGrid.RefreshGrid();
                }
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
                        VerifyRecordResultHelper.GetMessageString(checkedResult), MessageBox.HiddenAsync);
                    thisView.NeedRefresh();
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
                        VerifyRecordResultHelper.GetMessageString(checkedResult), MessageBox.HiddenAsync);
                    thisView.NeedRefresh();
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
        public void OnOpenUserPicker()
        {
            ShowUserPicker = true;
        }

        public void OnPickerUserCompletion(MyUserAdapterModel e)
        {
            if (e != null)
            {
                CurrentRecord.EngineerId = e.Id;
                CurrentRecord.EngineerName = e.Name;
            }
            ShowUserPicker = false;
        }

        public void OnWorkOrderSendingDialog()
        {
            ShowWorkOrderSendingDialog = true;
        }

        public async Task OnWorkOrderSendingDialogCompletion(ApproveOpinionModel e)
        {
            if (e != null)
            {
                #region 產生一筆稽核送審記錄
                var user = await CurrentUserHelper.GetCurrentUserAsync();
                var code = UniqueStringHelper.GetCode();
                FlowMasterAdapterModel flowMasterAdapterModel = new FlowMasterAdapterModel()
                {
                    Code = code,
                    MyUserId = user.Id,
                    PolicyHeaderId = e.PolicyHeaderAdapterModel.Id,
                    CreateDate = DateTime.Now,
                    ProcessLevel = 0,
                    Title = $"工單完工 - {CurrentRecord.Description}",
                    Content = "",
                    Status = 0,
                    SourceType = FlowSourceTypeEnum.WorkOrder,
                    SourceJson = JsonConvert.SerializeObject(CurrentRecord),
                    SourceCode = CurrentRecord.Code,
                };

                flowMasterAdapterModel.UpdateAt = DateTime.Now;
                await FlowMasterService.AddAsync(flowMasterAdapterModel);
                flowMasterAdapterModel = await FlowMasterService.GetAsync(code);
                await FlowMasterService.SendAsync(flowMasterAdapterModel, e);
                CurrentRecord.Status = MagicHelper.WorkOrderStatus送審;
                await CurrentService.UpdateAsync(CurrentRecord);
                this.dataGrid.RefreshGrid();
                #endregion
            }
            ShowWorkOrderSendingDialog = false;
        }
        #endregion

        #region 排序搜尋事件

        public void SortChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, SortCondition> args)
        {
            if (dataGrid.GridIsExist() == true)
            {
                CurrentSortCondition.Id = args.Value;
                dataGrid.RefreshGrid();
            }
        }

        public async Task WorkOrderStatusChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, WorkOrderStatusCondition> args)
        {
            if (args.IsInteracted == true)
            {
                if (dataGrid.GridIsExist() == true)
                {
                    CurrentWorkOrderStatusCondition.Id = args.Value;
                    CurrentWorkOrderStatusCondition.Title = WorkOrderStatusConditions
                        .FirstOrDefault(x => x.Id == CurrentWorkOrderStatusCondition.Id).Title;
                    await Task.Delay(200);
                    dataGrid.RefreshGrid();
                }
            }
        }

        public async Task CategoryMainChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, CategoryMainAdapterModel> args)
        {
            if (args.IsInteracted == true)
            {
                if (dataGrid.GridIsExist() == true)
                {
                    CurrentRecord.CategoryMainId = args.Value;
                    CurrentRecord.CategoryMainName = args.ItemData.Name;
                    if (args.PreviousItemData != null)
                    {
                        if (args.PreviousItemData.Id != args.ItemData.Id)
                        {
                            await GetCategorySubAdapterModels(CurrentRecord.CategoryMainId);
                            CurrentRecord.CategorySubId = 0;
                            CurrentRecord.CategorySubName = "";
                        }
                    }
                    else
                    {
                        await GetCategorySubAdapterModels(CurrentRecord.CategoryMainId);
                        CurrentRecord.CategorySubId = 0;
                        CurrentRecord.CategorySubName = "";
                    }
                }
            }
        }

        public async Task CategorySubChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, CategorySubAdapterModel> args)
        {
            if (args.IsInteracted == true)
            {
                if (dataGrid.GridIsExist() == true)
                {
                    CurrentRecord.CategorySubId = args.Value;
                    CurrentRecord.CategorySubName = args.ItemData.Name;
                    await Task.Yield();
                    thisView.NeedRefresh();
                }
            }
        }

        #endregion

        #region 送出
        public async Task SendAsync(WorkOrderAdapterModel workOrderAdapterModel)
        {
            if (workOrderAdapterModel.Status == MagicHelper.WorkOrderStatus完工)
            {
                var flowMasterAdapterModel = await FlowMasterService.GetSourceCodeAsync(workOrderAdapterModel.Code);
                if (flowMasterAdapterModel != null)
                {
                    await Task.Yield();
                    var checkTask = ConfirmMessageBox.ShowAsync("400px", "200px", "確認",
                         "這筆工單已經有送審過了，是否還要繼續送審", ConfirmMessageBox.HiddenAsync);
                    thisView.NeedRefresh();
                    var checkAgain = await checkTask;
                    if (checkAgain == false)
                    {
                        return;
                    }
                }
                CurrentRecord = workOrderAdapterModel;
                OnWorkOrderSendingDialog();
            }
            else
            {
                MessageBox.Show("400px", "200px", "警告", "派工單狀態必須是在完工狀態才可以送審",
                    MessageBox.HiddenAsync);
                await Task.Yield();
                thisView.NeedRefresh();
            }
        }
        #endregion

        #region DropList 相關方法
        public async Task GetCategoryMainAdapterModels()
        {
            if (CategoryMainAdapterModels.Count == 0)
            {
                CategoryMainAdapterModels = new List<CategoryMainAdapterModel>();
                var Items = await CategoryMainService.GetAsync();
                CategoryMainAdapterModels.AddRange(Items);
            }
        }
        public async Task GetCategorySubAdapterModels(int categoryMainId)
        {
            CategorySubAdapterModels = new List<CategorySubAdapterModel>();
            thisView.NeedRefresh();
            var Items = await CategorySubService.GetByHeaderIDAsync(categoryMainId,
                new DataRequest()
                {
                    Sorted = new SortCondition()
                    {
                        Id = (int)CategorySubSortEnum.OrderNumberAscending
                    },
                    Skip = 0,
                    Take = 0,
                });
            var itemAdapterModels = Items.Result.ToList();
            if (itemAdapterModels.Count > 0)
            {
                CategorySubAdapterModels.AddRange(itemAdapterModels);
            }
        }

        #endregion

        #region 互動式彈出按鈕事件
        public async Task OnContextMenuClick(ContextMenuClickEventArgs<WorkOrderAdapterModel> args)
        {
            CurrentRecord = args.RowInfo.RowData;
            if (args.Item.Id == "查看送審記錄")
            {
                var flowMasterAdapterModel = await FlowMasterService.GetSourceCodeAsync(CurrentRecord.Code);
                if(flowMasterAdapterModel!=null)
                {
                    CurrentFlowMasterAdapterModel = flowMasterAdapterModel;
                    ShowReviewFlowDialog = true;
                }
            }
            else if (args.Item.Id == "送審")
            {
                await SendAsync(CurrentRecord);
            }
        }

        public void OnReviewFlowDialogCompletion(object e)
        {
            ShowReviewFlowDialog = false;
        }
        #endregion
        #endregion
    }
}
