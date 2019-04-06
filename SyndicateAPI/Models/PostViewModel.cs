using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;

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
        public CoordinatesViewModel Coordinates { get; set; }

        public PostViewModel() { }

        public PostViewModel(Post post)
        {
            if (post != null)
            {
                ID = post.ID;
                Text = post.Text;
                PublishTime = post.PublishTime;
                Image = new FileViewModel(post.Image);
                MinRatingLevel = new RatingLevelViewModel(post.MinRatingLevel);
                Author = new UserViewModel(post.Author);
                Type = post.Type;
                IsPublished = post.IsPublished;
                Coordinates = new CoordinatesViewModel(post.Latitude, post.Longitude);
            }
        }
    }
}
