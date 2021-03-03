using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class TravelExpenseDto
    {
        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        public decimal TotalExpense { get; set; }

        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserDto MyUser { get; set; }
    }
}
