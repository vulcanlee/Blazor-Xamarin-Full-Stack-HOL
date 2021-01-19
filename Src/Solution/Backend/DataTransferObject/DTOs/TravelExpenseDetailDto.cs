using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public partial class TravelExpenseDetailDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "主題 不可為空白")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public decimal Expense { get; set; }

        public int TravelExpenseId { get; set; }
        public virtual TravelExpenseDto TravelExpense { get; set; }
    }
}
