using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class LogCategories
    {
        public static string User { get; set; } = "使用者";
        public static string SMTP { get; set; } = "SMTP服務";
        public static string Initialization { get; set; } = "資料庫重建";
        public static string Ohter { get; set; } = "其他";
    }
}
