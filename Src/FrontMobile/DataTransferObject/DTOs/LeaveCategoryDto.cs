using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class LeaveCategoryDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "請假類別 不可為空白")]
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LeaveCategoryDto Clone()
        {
            return ((ICloneable)this).Clone() as LeaveCategoryDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
