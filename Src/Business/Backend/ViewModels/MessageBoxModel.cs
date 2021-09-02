using System;
using System.Threading.Tasks;

namespace Backend.ViewModels
{
    public class MessageBoxModel
    {
        public bool IsVisible { get; set; } = false;
        public string Height { get; set; } = "250px";
        public string Width { get; set; } = "435px";
        public string Title { get; set; } = "通知訊息";
        public string Body { get; set; } = "訊息內容";
        public Func<Task> MessageCallbackDelegate { get; set; }
        public TaskCompletionSource TaskCompletionSource { get; set; }

        public void Show(string width, string height, string title, string body,
            Func<Task> messageCallbackDelegate = null)
        {
            TaskCompletionSource = null;
            MessageCallbackDelegate = messageCallbackDelegate;
            Height = height;
            Width = width;
            Title = title;
            Body = body;
            IsVisible = true;
        }

        public Task ShowAsync(string width, string height, string title, string body,
            Func<Task> messageCallbackDelegate = null)
        {
            MessageCallbackDelegate = messageCallbackDelegate;
            TaskCompletionSource = new TaskCompletionSource();
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

        public Task HiddenAsync()
        {
            if (TaskCompletionSource != null)
                TaskCompletionSource.SetResult();
            Hidden();
            return Task.CompletedTask;
        }
    }
}
