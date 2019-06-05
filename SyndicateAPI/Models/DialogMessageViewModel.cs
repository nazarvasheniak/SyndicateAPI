using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class DialogMessageViewModel
    {
        public long ID { get; set; }
        public DialogMessageType Type { get; set; }
        public UserViewModel Sender { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsReaded { get; set; }

        public DialogMessageViewModel() { }

        public DialogMessageViewModel(DialogMessage message, bool isSender)
        {
            if (message != null)
            {
                ID = message.ID;
                Type = message.Type;
                Sender = new UserViewModel(message.Sender);
                Content = message.Content;
                Time = message.Time;
                IsReaded = message.IsReaded;

                if (!isSender) InvertType();
            }
        }

        private void InvertType()
        {
            switch (Type)
            {
                case DialogMessageType.Incoming:
                    Type = DialogMessageType.Outgoing;
                    break;

                case DialogMessageType.Outgoing:
                    Type = DialogMessageType.Incoming;
                    break;
            }
        }
    }
}
