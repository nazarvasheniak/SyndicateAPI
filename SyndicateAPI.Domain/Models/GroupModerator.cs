using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class GroupModerator : PersistentObject, IDeletableObject
    {
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
        public virtual GroupModeratorLevel Level { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
