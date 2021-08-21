using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domains.Models
{
    public class PolicyDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public int Level { get; set; }
        public bool OnlyCC { get; set; }
        public bool Enable { get; set; }
        public int PolicyHeaderId { get; set; }
        public PolicyHeader PolicyHeader { get; set; }
        public int MyUserId { get; set; }
        public MyUser MyUser { get; set; }
    }
}
