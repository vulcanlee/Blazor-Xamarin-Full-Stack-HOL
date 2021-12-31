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
        public ConcurrentDictionary<Guid, Action<string>> AllMessageEvent { get; set; } = new ConcurrentDictionary<Guid, Action<string>>();
        public ILogger<SystemBroadcast> Logger { get; }

        public SystemBroadcast(ILogger<SystemBroadcast> logger)
        {
            Logger = logger;
        }
        public void Subscribe(Guid guid, Action<string> action)
        {
            AllMessageEvent.TryAdd(guid, action);
            return;
        }
        public void Unsubscribe(Guid guid)
        {
            Action<string> action;
            var foo = AllMessageEvent.TryGetValue(guid, out action);
            if (foo == true)
            {
               var success =  AllMessageEvent.TryRemove(guid, out action);
                if(success == false)
                {
                    //Logger.LogError($"移除 AllMessageEvent ({guid}) 失敗");
                }
            }
            else
            {
                //Logger.LogError($"沒有發現要移除 AllMessageEvent ({guid})");

            }
            return;
        }
        public void Publish(string message)
        {
            foreach (var item in AllMessageEvent)
            {
                try
                {
                    item.Value?.Invoke(message);
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
            int all = AllMessageEvent.Count();
            return all;
        }
    }
}
