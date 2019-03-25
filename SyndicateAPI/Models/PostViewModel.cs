using SyndicateAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class PostViewModel
    {
        public long ID { get; set; }
        public string Text { get; set; }
        public DateTime PublishTime { get; set; }
        public FileViewModel Image { get; set; }
        public RatingLevelViewModel MinRatingLevel { get; set; }
        public UserViewModel Author { get; set; }
        public PostType Type { get; set; }
        public bool IsPublished { get; set; }
    }
}
