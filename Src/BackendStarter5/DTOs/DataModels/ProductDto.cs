using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class ProductDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "商品名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public ProductDto Clone()
        {
            return ((ICloneable)this).Clone() as ProductDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
