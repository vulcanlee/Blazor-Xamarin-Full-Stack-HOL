using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domains.Models
{
    public class FlowUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MyUserId { get; set; }
        public MyUser MyUser { get; set; }
        public int Level { get; set; }
        public bool OnlyCC { get; set; }
        /// <summary>
        /// 簽核流程已經完成
        /// </summary>
        public bool Completion { get; set; }
        public bool Enable { get; set; }
        public int FlowMasterId { get; set; }
        public FlowMaster FlowMaster { get; set; }
    }
}
