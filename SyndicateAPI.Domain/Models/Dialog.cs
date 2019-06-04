using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Dialog : PersistentObject, IDeletableObject
    {
        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
