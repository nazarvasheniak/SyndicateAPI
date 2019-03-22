﻿using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Person : PersistentObject, IDeletableObject
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Biography { get; set; }
        public virtual City City { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
