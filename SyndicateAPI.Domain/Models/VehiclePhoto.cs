using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class VehiclePhoto : PersistentObject, IDeletableObject
    {
        public virtual File Photo { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
