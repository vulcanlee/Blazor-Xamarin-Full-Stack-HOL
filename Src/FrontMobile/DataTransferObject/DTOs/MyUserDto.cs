﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class MyUserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public int ServiceId { get; set; }
        public bool IsService { get; set; }
        public string ServiceName { get; set; }
        public string IsServiceString { get; set; }
    }
}
