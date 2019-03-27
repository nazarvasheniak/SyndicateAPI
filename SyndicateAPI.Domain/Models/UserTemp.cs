using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class UserTemp : PersistentObject, IDeletableObject
    {
        public virtual User User { get; set; }
        public virtual string Email { get; set; }
        public virtual int TempCode { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
