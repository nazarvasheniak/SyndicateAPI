using SyndicateAPI.Domain.Interfaces;
using System;

namespace SyndicateAPI.Domain.Models
{
    public class UserSession : PersistentObject, IDeletableObject
    {
        public virtual User User { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
