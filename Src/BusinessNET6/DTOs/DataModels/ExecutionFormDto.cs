using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class ExecutionFormDto : ICloneable, INotifyPropertyChanged
    {
        public virtual ICollection<ExecutionItemDto> ExecutionItem { get; set; }
        public ExecutionFormDto()
        {
            ExecutionItem = new HashSet<ExecutionItemDto>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int SortNumber { get; set; }
        [Required(ErrorMessage = "項目名稱 不可為空白")]
        public string Name { get; set; }
        /// <summary>
        /// 說明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 地點
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// NFC 標籤
        /// </summary>
        public string NfcCode { get; set; }
        public string QRCode { get; set; }
        /// <summary>
        /// 分類標籤
        /// </summary>
        public string Tags { get; set; }
        public int ExecutionPlanId { get; set; }
        public ExecutionPlanDto ExecutionPlan { get; set; }
        public string ExecutionPlanName { get; set; }
        public int FormHeaderId { get; set; }
        public FormHeaderDto FormHeader { get; set; }
        public string FormHeaderName { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public ExecutionFormDto Clone()
        {
            return ((ICloneable)this).Clone() as ExecutionFormDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
