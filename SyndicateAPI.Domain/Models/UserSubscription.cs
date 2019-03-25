using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class UserSubscription : PersistentObject, IDeletableObject
    {
        public virtual User Subject { get; set; }
        public virtual User Subscriber { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
