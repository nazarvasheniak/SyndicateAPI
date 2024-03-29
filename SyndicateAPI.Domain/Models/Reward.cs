﻿using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class Reward : PersistentObject, IDeletableObject
    {
        public virtual string Name { get; set; }
        public virtual File Icon { get; set; }
        public virtual User User { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
