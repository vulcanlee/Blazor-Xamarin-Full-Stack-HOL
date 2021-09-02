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

    public class FlowMasterViewModel
    {
        #region Constructor
        public FlowMasterViewModel(IFlowMasterService CurrentService,
           BackendDBContext context, IMapper Mapper,
           UserHelper currentUserHelper,
           TranscationResultHelper transcationResultHelper,
           CurrentUser currentUser)
        {
            this.CurrentService = CurrentService;
            this.context = context;
            mapper = Mapper;
            CurrentUserHelper = currentUserHelper;
            TranscationResultHelper = transcationResultHelper;
            CurrentUser = currentUser;
            FlowMasterSort.Initialization(SortConditions);

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
        /// <summary>
        /// 對選取紀錄進行 新增 或者 修改 
        /// </summary>
        public bool IsNewRecordMode { get; set; } = false;
        /// <summary>
        /// 是否要顯示紀錄新增或修改對話窗 
        /// </summary>
        public bool IsShowEditRecord { get; set; } = false;
        /// <summary>
        /// 是否要顯示關聯多筆資料表的 CRUD 對話窗
        /// </summary>
        public bool IsShowMoreDetailsRecord { get; set; } = false;
        public bool IsShowFlowUserRecord { get; set; } = false;
        public bool IsShowFlowHistoryRecord { get; set; } = false;
        /// <summary>
        /// 現在正在新增或修改的紀錄  
        /// </summary>
        public FlowMasterAdapterModel CurrentRecord { get; set; } = new FlowMasterAdapterModel();
        /// <summary>
        /// 現在正在刪除的紀錄  
        /// </summary>
        public FlowMasterAdapterModel CurrentNeedDeleteRecord { get; set; } = new FlowMasterAdapterModel();
        /// <summary>
        /// 保存與資料編輯程式相關的中繼資料
        /// </summary>
        public EditContext LocalEditContext { get; set; }
        /// <summary>
        /// 是否顯示選取其他清單記錄對話窗 
        /// </summary>
        public bool ShowAontherRecordPicker { get; set; } = false;
        public bool ShowSimulatorUserPicker { get; set; } = false;
        public bool ShowPolicyRecordPicker { get; set; } = false;
        public bool ShowApproveOpinionDialog { get; set; } = false;
        public FlowActionEnum FlowActionEnum { get; set; }
        public FlowMasterAdapterModel FlowMasterAdapterModel { get; set; }
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
        public IDataGrid ShowFlowUserGrid { get; set; }
        public IDataGrid ShowFlowHistoryGrid { get; set; }
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
        public bool IsGod { get; set; } = false;

        #region 訊息說明之對話窗使用的變數
        /// <summary>
        /// 確認對話窗設定
        /// </summary>
        public ConfirmBoxModel ConfirmMessageBox { get; set; } = new ConfirmBoxModel();
        /// <summary>
        /// 訊息對話窗設定
        /// </summary>
        public MessageBoxModel MessageBox { get; set; } = new MessageBoxModel();
        public UserHelper CurrentUserHelper { get; }
        public TranscationResultHelper TranscationResultHelper { get; }
        public CurrentUser CurrentUser { get; }
        #endregion
        #endregion

        #region Field
        /// <summary>
        /// 當前記錄需要用到的 Service 物件 
        /// </summary>
        private readonly IFlowMasterService CurrentService;
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
                CurrentRecord = new FlowMasterAdapterModel();
                #region 針對新增的紀錄所要做的初始值設定商業邏輯
                #endregion
                EditRecordDialogTitle = "新增紀錄";
                IsNewRecordMode = true;
                IsShowEditRecord = true;
                var user = await CurrentUserHelper.GetCurrentUserAsync();
                CurrentRecord.MyUserId = user.Id;
                CurrentRecord.MyUserName = user.Name;
                CurrentRecord.Status = 0;
                CurrentRecord.ProcessLevel = 0;
                CurrentRecord.Code = UniqueStringHelper.GetCode();
                CurrentRecord.CreateDate = DateTime.Now;
            }
            else if (args.Item.Id == ButtonIdHelper.ButtonIdRefresh)
            {
                dataGrid.RefreshGrid();
            }
        }
        #endregion

        #region 記錄列的按鈕事件 (修改與刪除與明細紀錄瀏覽)
        public async Task OnCommandClicked(CommandClickEventArgs<FlowMasterAdapterModel> args)
        {
            FlowMasterAdapterModel item = args.RowData as FlowMasterAdapterModel;
            if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdEdit)
            {
                #region 點選 修改紀錄 按鈕
                CurrentRecord = item.Clone();
                EditRecordDialogTitle = "修改紀錄";
                IsShowEditRecord = true;
                IsNewRecordMode = false;
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
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(checkedResult.MessageId), HiddenMessageBox);
                    await Task.Yield();
                    thisView.NeedRefresh();
                    return;
                }
                #endregion

                ConfirmMessageBox.Show("400px", "200px", "警告", "確認要刪除這筆紀錄嗎？");
                #endregion
            }
            else if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdShowFlowUser)
            {
                #region 點選 稽核使用者 對話窗 按鈕
                IsShowFlowUserRecord = true;
                ShowMoreDetailsRecordDialogTitle = MagicHelper.簽核使用者明細;
                MasterRecord masterRecord = new MasterRecord()
                {
                    Id = item.Id
                };
                Header = masterRecord;
                if (ShowFlowUserGrid != null)
                {
                    await Task.Delay(100); // 使用延遲，讓 Header 的資料綁定可以成功
                    ShowFlowUserGrid.RefreshGrid();
                }
                #endregion
            }
            else if (args.CommandColumn.ButtonOption.IconCss == ButtonIdHelper.ButtonIdShowFlowHistory)
            {
                #region 點選 開啟多筆 CRUD 對話窗 按鈕
                IsShowFlowHistoryRecord = true;
                ShowMoreDetailsRecordDialogTitle = MagicHelper.簽核歷史紀錄;
                MasterRecord masterRecord = new MasterRecord()
                {
                    Id = item.Id
                };
                Header = masterRecord;
                if (ShowFlowHistoryGrid != null)
                {
                    await Task.Delay(100); // 使用延遲，讓 Header 的資料綁定可以成功
                    ShowFlowHistoryGrid.RefreshGrid();
                }
                #endregion
            }
        }

        public async Task RemoveThisRecord(bool NeedDelete)
        {
            if (NeedDelete == true)
            {
                var verifyRecordResult = await CurrentService.DeleteAsync(CurrentNeedDeleteRecord.Id);
                await TranscationResultHelper.CheckDatabaseResult(MessageBox, verifyRecordResult);
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
            if (IsNewRecordMode == true)
            {
                var checkedResult = await CurrentService
                    .BeforeAddCheckAsync(CurrentRecord);
                if (checkedResult.Success == false)
                {
                    MessageBox.Show("400px", "200px", "警告",
                        VerifyRecordResultHelper.GetMessageString(checkedResult), HiddenMessageBox);
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
                        VerifyRecordResultHelper.GetMessageString(checkedResult), HiddenMessageBox);
                    thisView.NeedRefresh();
                    return;
                }
            }
            #endregion

            if (IsShowEditRecord == true)
            {
                if (IsNewRecordMode == true)
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
        public void OnOpenPicker()
        {
            ShowAontherRecordPicker = true;
        }
        public void OnPickerCompletion(MyUserAdapterModel e)
        {
            if (e != null)
            {
                CurrentRecord.MyUserId = e.Id;
                CurrentRecord.MyUserName = e.Name;
            }
            ShowAontherRecordPicker = false;
        }
        public void OnOpenSimulatorUserPicker()
        {
            ShowSimulatorUserPicker = true;
        }
        public void OnPickerSimulatorUserCompletion(MyUserAdapterModel e)
        {
            if (e != null)
            {
                CurrentUserHelper.CustomUserId = e.Id;
                CurrentUserHelper.CustomUserName = e.Name;
                CurrentUser.CurrentMyUserId = e.Id;
                CurrentUser.SimulatorMyUserAdapterModel = e;
                dataGrid.RefreshGrid();
            }
            else
            {
                CurrentUserHelper.CustomUserId = 0;
                CurrentUserHelper.CustomUserName = "";
                CurrentUser.CurrentMyUserId = 0;
                CurrentUser.SimulatorMyUserAdapterModel = null;
            }
            ShowSimulatorUserPicker = false;
        }
        public void OnOpenPolicyPicker()
        {
            ShowPolicyRecordPicker = true;
        }
        public void OnPickerPolicyCompletion(PolicyHeaderAdapterModel e)
        {
            if (e != null)
            {
                CurrentRecord.PolicyHeaderId = e.Id;
                CurrentRecord.PolicyHeaderName = e.Name;
            }
            ShowPolicyRecordPicker = false;
        }
        public void OnShowApproveOpinionDialog()
        {
            ShowApproveOpinionDialog = true;
        }
        public async Task OnShowApproveOpinionDialogCompletion(ApproveOpinionModel e)
        {
            if (e != null)
            {
                switch (FlowActionEnum)
                {
                    case FlowActionEnum.Send:
                        await CurrentService.SendAsync(FlowMasterAdapterModel, e);
                        break;
                    case FlowActionEnum.BackToSend:
                        await CurrentService.BackToSendAsync(FlowMasterAdapterModel, e);
                        break;
                    case FlowActionEnum.Agree:
                        await CurrentService.AgreeAsync(FlowMasterAdapterModel, e);
                        break;
                    case FlowActionEnum.Deny:
                        await CurrentService.DenyAsync(FlowMasterAdapterModel, e);
                        break;
                    default:
                        break;
                }
                dataGrid.RefreshGrid();
            }
            else
            {
            }
            ShowApproveOpinionDialog = false;
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

        #endregion

        #region 審核動作事件
        #region 送出
        public async Task SendAsync(FlowMasterAdapterModel flowMasterAdapterModel)
        {
            OnShowApproveOpinionDialog();
            await Task.Yield();
            FlowMasterAdapterModel = flowMasterAdapterModel;
            FlowActionEnum = FlowActionEnum.Send;
        }
        #endregion

        #region 退回申請者
        public async Task BackToSendAsync(FlowMasterAdapterModel flowMasterAdapterModel)
        {
            OnShowApproveOpinionDialog();
            await Task.Yield();
            FlowMasterAdapterModel = flowMasterAdapterModel;
            FlowActionEnum = FlowActionEnum.BackToSend;
        }
        #endregion

        #region 同意
        public async Task AgreeAsync(FlowMasterAdapterModel flowMasterAdapterModel)
        {
            OnShowApproveOpinionDialog();
            await Task.Yield();
            FlowMasterAdapterModel = flowMasterAdapterModel;
            FlowActionEnum = FlowActionEnum.Agree;
        }
        #endregion

        #region 退回
        public async Task DenyAsync(FlowMasterAdapterModel flowMasterAdapterModel)
        {
            OnShowApproveOpinionDialog();
            await Task.Yield();
            FlowMasterAdapterModel = flowMasterAdapterModel;
            FlowActionEnum = FlowActionEnum.Deny;
        }
        #endregion

        #region 檢查該使用者可以做批示動作
        public bool CheckFlowAction(FlowMasterAdapterModel flowMasterAdapterModel)
        {
            var result = Task.Run<bool>(async () =>
            {
                var result = await CurrentService.CheckUserShowActionAsync(flowMasterAdapterModel, CurrentUser);
                return result;
            }).Result;
            return result;
        }
        #endregion
        #endregion

        #region 訊息與確認對話窗方法
        public Task HiddenMessageBox()
        {
            MessageBox.Hidden();
            return Task.CompletedTask;
        }

        #endregion
        #endregion
    }
}
