using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ToastMessageModel
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public int Timeout { get; set; } = 5000;
        public string Color { get; set; } = "dodgerblue";
    }
}
