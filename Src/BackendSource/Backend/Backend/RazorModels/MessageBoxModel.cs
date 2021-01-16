using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.RazorModels
{
    public class MessageBoxModel
    {
        public bool IsVisible { get; set; } = false;
        public string Height { get; set; } = "250px";
        public string Width { get; set; } = "435px";
        public string Title { get; set; } = "通知訊息";
        public string Body { get; set; } = "訊息內容";

        public void Show(string width, string height, string title, string body)
        {
            Height = height;
            Width = width;
            Title = title;
            Body = body;
            IsVisible = true;
        }

        public void Hidden()
        {
            IsVisible = false;
        }
    }
}
