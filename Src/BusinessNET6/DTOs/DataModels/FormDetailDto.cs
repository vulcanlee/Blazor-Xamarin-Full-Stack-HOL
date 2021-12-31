using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class FormDetailDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// 檢驗項目的分類名稱
        /// </summary>
        public string Category { get; set; }
        [Required(ErrorMessage = "項目名稱 不可為空白")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enable { get; set; }
        public bool? Abnormal { get; set; }
        public string Comment { get; set; }
        public int FormHeaderId { get; set; }
        public FormHeaderDto FormHeader { get; set; }
        public string FormHeaderName { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public FormDetailDto Clone()
        {
            return ((ICloneable)this).Clone() as FormDetailDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
