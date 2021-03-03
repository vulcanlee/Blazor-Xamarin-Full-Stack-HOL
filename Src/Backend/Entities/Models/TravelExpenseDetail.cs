using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 差旅費用申請明細
    /// </summary>
    public class TravelExpenseDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "主題 不可為空白")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public decimal Expense { get; set; }

        public int TravelExpenseId { get; set; }
        public virtual TravelExpense TravelExpense { get; set; }
    }
}
