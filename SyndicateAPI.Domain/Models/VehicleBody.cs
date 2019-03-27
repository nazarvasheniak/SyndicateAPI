using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class VehicleBody : PersistentObject, IDeletableObject
    {
        public virtual string Title { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
