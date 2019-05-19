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
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsReaded { get; set; }
        public UserViewModel Sender { get; set; }

        public DialogMessageViewModel() { }

        public DialogMessageViewModel(DialogMessage message)
        {
            if (message != null)
            {
                ID = message.ID;
                Content = message.Content;
                Time = message.Time;
                IsReaded = message.IsReaded;
                Sender = new UserViewModel(message.Sender);
            }
        }
    }
}
