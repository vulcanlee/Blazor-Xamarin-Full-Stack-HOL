using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Events
{
    public class ToastEvent : PubSubEvent<ToastPayload>
    {

    }

    public class ToastPayload
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public int Timeout { get; set; } = 3000;
        public string Color { get; set; } = "dodgerblue";
    }
}
