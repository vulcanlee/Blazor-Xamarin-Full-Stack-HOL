using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.DataModels
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "商品名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }
    }
}
