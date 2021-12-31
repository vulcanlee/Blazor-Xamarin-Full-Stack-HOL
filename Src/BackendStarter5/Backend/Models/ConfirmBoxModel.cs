using System;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ConfirmBoxModel
    {
        public bool IsVisible { get; set; } = false;
        public string Height { get; set; } = "200px";
        public string Width { get; set; } = "400px";
        public string Title { get; set; } = "警告";
        public string Body { get; set; } = "確認要刪除這筆紀錄嗎？";
        public Func<bool, Task> ConfirmDelegate { get; set; }
        public TaskCompletionSource<bool> TaskCompletionSource { get; set; }
        public void Show(string width, string height, string title, string body,
            Func<bool, Task> confirmDelegate = null)
        {
            ConfirmDelegate = confirmDelegate;
            TaskCompletionSource = null;
            Height = height;
            Width = width;
            Title = title;
            Body = body;
            IsVisible = true;
        }

        public Task<bool> ShowAsync(string width, string height, string title, string body,
            Func<bool, Task> confirmDelegate = null)
        {
            ConfirmDelegate = confirmDelegate;
            TaskCompletionSource = new TaskCompletionSource<bool>();
            Height = height;
            Width = width;
            Title = title;
            Body = body;
            IsVisible = true;
            return TaskCompletionSource.Task;
        }

        public void Hidden()
        {
            IsVisible = false;
        }

        public Task HiddenAsync(bool choise)
        {
            if (TaskCompletionSource != null)
                TaskCompletionSource.SetResult(choise);
            Hidden();
            return Task.CompletedTask;
        }
    }
}
