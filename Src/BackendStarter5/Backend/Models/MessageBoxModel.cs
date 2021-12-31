using Backend.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class MessageBoxModel
    {
        public bool IsVisible { get; set; } = false;
        public string Height { get; set; } = "250px";
        public string Width { get; set; } = "435px";
        public string Title { get; set; } = "通知訊息";
        public string Body { get; set; } = "訊息內容";
        public Func<Task> MessageDelegate { get; set; }
        public TaskCompletionSource TaskCompletionSource { get; set; }
        public int TimeElapsing { get; set; }
        public CancellationTokenSource CTS { get; set; }
        public Task WaitTask { get; set; }
        public IRazorPage RazorPage { get; set; }

        public void Show(string width, string height, string title, string body,
            Func<Task> messageCallbackDelegate = null, int timeElapsing = 0, IRazorPage razorPage = null)
        {
            TimeElapsing = timeElapsing;
            RazorPage = razorPage;
            TaskCompletionSource = null;
            MessageDelegate = messageCallbackDelegate;
            Height = height;
            Width = width;
            Title = title;
            Body = body;
            IsVisible = true;
            if (TimeElapsing > 0 && RazorPage != null)
            {
                WaitTask = Task.Run(async () =>
                {
                    int timeCounter = 0;
                    while (true)
                    {
                        if (timeCounter > TimeElapsing)
                        {
                            if (messageCallbackDelegate != null)
                                await messageCallbackDelegate();
                            await RazorPage.NeedRefreshAsync();
                            break;
                        }
                        await Task.Delay(500);
                        timeCounter += 500;
                    }
                });
            }
        }

        public Task ShowAsync(string width, string height, string title, string body,
            Func<Task> messageCallbackDelegate = null, int timeElapsing = 0, IRazorPage razorPage = null)
        {
            TimeElapsing = timeElapsing;
            RazorPage = razorPage;
            MessageDelegate = messageCallbackDelegate;
            TaskCompletionSource = new TaskCompletionSource();
            Height = height;
            Width = width;
            Title = title;
            Body = body;
            IsVisible = true;
            if (TimeElapsing > 0 && RazorPage != null)
            {
                WaitTask = Task.Run(async () =>
                {
                    int timeCounter = 0;
                    while (true)
                    {
                        if (timeCounter > TimeElapsing)
                        {
                            if (messageCallbackDelegate != null)
                                await messageCallbackDelegate();
                            await RazorPage.NeedRefreshAsync();
                            break;
                        }
                        await Task.Delay(500);
                        timeCounter += 500;
                    }
                });
            }
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
