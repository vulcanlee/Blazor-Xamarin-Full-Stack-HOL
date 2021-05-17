using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class ProductAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "商品名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }

        public ProductAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as ProductAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
