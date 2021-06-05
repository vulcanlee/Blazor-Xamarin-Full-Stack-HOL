using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Events
{
    public class SystemBroadcast
    {
        public ConcurrentBag<Action<string>> AllNewQuestionEvent { get; set; } = new ConcurrentBag<Action<string>>();
        public ConcurrentBag<Action<int>> ConcurrentConnectionEvent { get; set; } = new ConcurrentBag<Action<int>>();
        public ILogger<SystemBroadcast> Logger { get; }

        public SystemBroadcast(ILogger<SystemBroadcast> logger)
        {
            Logger = logger;
        }
        public void Subscribe(Action<string> action)
        {
            AllNewQuestionEvent.Add(action);
            return;
        }
        public void Unsubscribe(Action<string> action)
        {
            var foo = AllNewQuestionEvent.FirstOrDefault(x => x == action);
            if (foo != null)
            {
                AllNewQuestionEvent.TryTake(out foo);
            }
            return;
        }
        public void Publish(string message)
        {
            foreach (var item in AllNewQuestionEvent)
            {
                try
                {
                    item?.Invoke(message);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Publish 廣播通知發生例外異常");
                }
            }
            return;
        }
        public int GetAllSubscriber()
        {
            int all = AllNewQuestionEvent.Count();
            return all;
        }
        public void SubscribeConcurrentConnection(Action<int> action)
        {
            ConcurrentConnectionEvent.Add(action);
            return;
        }
        public void UnsubscribeConcurrentConnection(Action<int> action)
        {
            var foo = ConcurrentConnectionEvent.FirstOrDefault(x => x == action);
            if (foo != null)
            {
                ConcurrentConnectionEvent.TryTake(out foo);
            }
            return;
        }
        public void PublishConcurrentConnection()
        {
            int concurrentConnection = ConcurrentConnectionEvent.Count();
            foreach (var item in ConcurrentConnectionEvent)
            {
                try
                {
                    item?.Invoke(concurrentConnection);
                }
                catch { }
            }
            return;
        }
    }
}
