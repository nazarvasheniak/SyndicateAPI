using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class PostCommentLike : PersistentObject, IDeletableObject
    {
        public virtual PostComment Comment { get; set; }
        public virtual User User { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
