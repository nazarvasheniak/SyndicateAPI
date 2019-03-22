﻿using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Group : PersistentObject, IDeletableObject
    {
        public virtual string Name { get; set; }
        public virtual string ShortDesc { get; set; }
        public virtual string FullDesc { get; set; }
        public virtual File Avatar { get; set; }
        public virtual User Owner { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
