using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class CRUDDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Updatetime { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public CRUDDto Clone()
        {
            return ((ICloneable)this).Clone() as CRUDDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
