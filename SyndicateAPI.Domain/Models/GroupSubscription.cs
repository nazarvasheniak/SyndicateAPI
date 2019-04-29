using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class GroupSubscription : PersistentObject, IDeletableObject
    {
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
