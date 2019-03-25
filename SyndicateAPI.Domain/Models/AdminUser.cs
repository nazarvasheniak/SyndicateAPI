using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class AdminUser : PersistentObject, IDeletableObject
    {
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual Person Person { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
