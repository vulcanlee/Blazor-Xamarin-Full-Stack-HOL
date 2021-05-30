﻿using Prism.Events;
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
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
