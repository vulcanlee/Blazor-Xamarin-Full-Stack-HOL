using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class ExecutionItemFileDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enable { get; set; }
        public string EnableName
        {
            get
            {
                if (Enable)
                {
                    return "啟用";
                }
                else
                {
                    return "停用";
                }
            }
        }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public ExecutionItemFileDto Clone()
        {
            return ((ICloneable)this).Clone() as ExecutionItemFileDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
