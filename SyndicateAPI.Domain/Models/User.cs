using SyndicateAPI.Domain.Interfaces;
using System;

namespace SyndicateAPI.Domain.Models
{
    public class User : PersistentObject, IDeletableObject
    {
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Nickname { get; set; }
        public virtual long PointsCount { get; set; }
        public virtual DateTime RegTime { get; set; }
        public virtual File Avatar { get; set; }
        public virtual Person Person { get; set; }
        public virtual Group Group { get; set; }
        public virtual int ActivationCode { get; set; }
        public virtual DateTime ActivationTime { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsOnline { get; set; }
        public virtual bool Deleted { get; set; }
    }
}