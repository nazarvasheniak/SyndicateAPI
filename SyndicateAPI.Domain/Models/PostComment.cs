using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class PostComment : PersistentObject, IDeletableObject
    {
        public virtual string Text { get; set; }
        public virtual Post Post { get; set; }
        public virtual User Author { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
