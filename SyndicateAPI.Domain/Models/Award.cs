using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Award : PersistentObject, IDeletableObject
    {
        public virtual User Awarder { get; set; }
        public virtual User Rewarder { get; set; }
        public virtual Reward Reward { get; set; }
        public virtual string Comment { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
