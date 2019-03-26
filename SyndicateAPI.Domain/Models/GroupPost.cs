using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class GroupPost : PersistentObject, IDeletableObject
    {
        public virtual Post Post { get; set; }
        public virtual Group Group { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
