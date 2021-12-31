using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class FormHeaderDto : ICloneable, INotifyPropertyChanged
    {
        public virtual ICollection<FormDetailDto> FormDetail { get; set; }
        public FormHeaderDto()
        {
            FormDetail = new HashSet<FormDetailDto>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 表單名稱
        /// </summary>
        [Required(ErrorMessage = "表單名稱 不可為空白")]
        public string Name { get; set; }
        /// <summary>
        /// 表單說明
        /// </summary>
        public string Description { get; set; }
        public bool Enable { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public FormHeaderDto Clone()
        {
            return ((ICloneable)this).Clone() as FormHeaderDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
