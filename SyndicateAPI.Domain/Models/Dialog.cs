using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Dialog : PersistentObject, IDeletableObject
    {
        public virtual User Participant1 { get; set; }
        public virtual User Participant2 { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
