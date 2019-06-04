using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class DialogMessage : PersistentObject, IDeletableObject
    {
        public virtual Dialog Dialog { get; set; }
        public virtual DialogMessageType Type { get; set; }
        public virtual User Sender { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual bool IsReaded { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
