using Prism.Events;
using Rehm2024.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm2024.Event
{
    public class MessageModel
    {
        public string Filter { get; set; }
        public string Message { get; set; }
    }

    public class MessageEvent : PubSubEvent<MessageModel>
    {
    }
    public class MessagesEvent : PubSubEvent<string>
    {
    }
    

    public class SettingEvent : PubSubEvent<AppSettings>
    {
    }
}
