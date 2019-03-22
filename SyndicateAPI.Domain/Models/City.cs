using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class City : PersistentObject, IDeletableObject
    {
        public virtual string Name { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
