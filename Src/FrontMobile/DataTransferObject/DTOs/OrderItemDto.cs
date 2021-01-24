using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class OrderItemDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [Required(ErrorMessage = "訂單項目名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }

        public string ProductName { get; set; }
        public string OrderName { get; set; }

        public OrderDto Order { get; set; }
        public ProductDto Product { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderItemDto Clone()
        {
            return ((ICloneable)this).Clone() as OrderItemDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
