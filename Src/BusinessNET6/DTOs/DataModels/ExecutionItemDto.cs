using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class ExecutionItemDto : ICloneable, INotifyPropertyChanged
    {
        public virtual ICollection<ExecutionItemFileDto> ExecutionItemFile { get; set; }
        public ExecutionItemDto()
        {
            ExecutionItemFile = new HashSet<ExecutionItemFileDto>();
        }

        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// 檢驗項目的分類名稱
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 檢核項目名稱
        /// </summary>
        [Required(ErrorMessage = "項目名稱 不可為空白")]
        public string Name { get; set; }
        /// <summary>
        /// 檢核項目說明
        /// </summary>
        public string Description { get; set; }
        public bool Enable { get; set; }
        /// <summary>
        /// 檢核項目是否異常
        /// </summary>
        public bool? Abnormal { get; set; }
        /// <summary>
        /// 檢核後的備註說明
        /// </summary>
        public string Comment { get; set; }
        public int FormDetailId { get; set; }
        public FormDetailDto FormDetail { get; set; }
        public string FormDetailName { get; set; }
        public int ExecutionFormId { get; set; }
        public ExecutionFormDto ExecutionForm { get; set; }
        public string ExecutionFormName { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public ExecutionItemDto Clone()
        {
            return ((ICloneable)this).Clone() as ExecutionItemDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
