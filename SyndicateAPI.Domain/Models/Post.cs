﻿using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class Post : PersistentObject, IDeletableObject
    {
        public virtual string Text { get; set; }
        public virtual DateTime PublishTime { get; set; }
        public virtual File Image { get; set; }
        public virtual RatingLevel MinRatingLevel { get; set; }
        public virtual User Author { get; set; }
        public virtual PostType Type { get; set; }
        public virtual bool IsPublished { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
