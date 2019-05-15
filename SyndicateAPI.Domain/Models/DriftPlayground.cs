using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class DriftPlayground : PersistentObject, IDeletableObject, IMapPointObject
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
