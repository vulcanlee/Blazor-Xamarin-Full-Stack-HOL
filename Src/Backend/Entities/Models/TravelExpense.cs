using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 差旅費用申請
    /// </summary>
    public class TravelExpense
    {
        public TravelExpense()
        {
            TravelExpenseDetail = new HashSet<TravelExpenseDetail>();
        }

        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        public decimal TotalExpense { get; set; }

        public int MyUserId { get; set; }
        public virtual MyUser MyUser { get; set; }

        public virtual ICollection<TravelExpenseDetail> TravelExpenseDetail { get; set; }
    }
}
