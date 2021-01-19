using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class WorkingLogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LogDate { get; set; }
        public decimal TotalHours { get; set; }
        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserDto MyUser { get; set; }
    }
}
