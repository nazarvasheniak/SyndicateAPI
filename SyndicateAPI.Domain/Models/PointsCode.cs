using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class PointsCode : PersistentObject, IDeletableObject
    {
        public virtual string Code { get; set; }
        public virtual long PointsCount { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ExpiresDate { get; set; }
        public virtual bool IsUsed { get; set; }
        public virtual User Creator { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
