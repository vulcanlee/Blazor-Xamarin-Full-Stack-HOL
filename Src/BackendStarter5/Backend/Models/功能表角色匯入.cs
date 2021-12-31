using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class 功能表角色匯入
    {
        public string 角色名稱 { get; set; }
        public string 名稱 { get; set; }
        public string 層級 { get; set; }
        public string 子功能表 { get; set; }
        public string 排序值 { get; set; }
        public string Icon名稱 { get; set; }
        public string 路由作業 { get; set; }
        public string 啟用 { get; set; }
        public string 強制導航 { get; set; }
    }
}
