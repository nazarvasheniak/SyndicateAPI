using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class PartnerProduct : PersistentObject, IDeletableObject
    {
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual long PointsCount { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
