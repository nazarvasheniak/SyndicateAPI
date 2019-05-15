using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Partner : PersistentObject, IDeletableObject, IMapPointObject
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual File Logo { get; set; }
        public virtual File MapIcon { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual MapPointType MapPointType { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
