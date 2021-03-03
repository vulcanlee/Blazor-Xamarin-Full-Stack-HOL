using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.RazorModels
{
    public class ConfirmBoxModel
    {
        public bool IsVisible { get; set; } = false;
        public string Height { get; set; } = "200px";
        public string Width { get; set; } = "400px";
        public string Title { get; set; } = "警告";
        public string Body { get; set; } = "確認要刪除這筆紀錄嗎？";
        public void Show(string width, string height, string title, string body, Func<bool> confirmDelegate = null)
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
