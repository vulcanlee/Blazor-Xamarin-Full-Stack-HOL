using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.DataModels
{
    public class CRUDDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Updatetime { get; set; }
    }
}
