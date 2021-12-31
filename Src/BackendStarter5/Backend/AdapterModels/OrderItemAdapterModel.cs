using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class OrderItemAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int OrderMasterId { get; set; }
        [Required(ErrorMessage = "訂單項目名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }

        public string ProductName { get; set; }
        public string OrderName { get; set; }

        public OrderMasterAdapterModel OrderMaster { get; set; }
        public ProductAdapterModel Product { get; set; }

        public OrderItemAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as OrderItemAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
