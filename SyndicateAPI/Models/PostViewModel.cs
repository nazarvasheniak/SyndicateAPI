using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyndicateAPI.Models
{
    public class PostViewModel
    {
        public long ID { get; set; }
        public string Text { get; set; }
        public DateTime PublishTime { get; set; }
        public FileViewModel Image { get; set; }
        public ulong RatingScore { get; set; }
        public UserViewModel Author { get; set; }
        public PostType Type { get; set; }
        public bool IsPublished { get; set; }
        public bool IsLiked { get; set; }
        public ulong LikesCount { get; set; }
        public List<PostCommentViewModel> Comments { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }
        public MapPointType MapPointType { get; set; }

        public PostViewModel() { }

        public PostViewModel(Post post)
        {
            if (post != null)
            {
                ID = post.ID;
                Text = post.Text;
                PublishTime = post.PublishTime;
                Image = new FileViewModel(post.Image);
                RatingScore = post.RatingScore;
                Author = new UserViewModel(post.Author);
                Type = post.Type;
                IsPublished = post.IsPublished;
                Coordinates = new CoordinatesViewModel(post.Latitude, post.Longitude);
                IsLiked = false;
                LikesCount = 0;
                Comments = new List<PostCommentViewModel>();
            }
        }

        public PostViewModel(Post post, IEnumerable<PostCommentViewModel> comments, bool isLiked, ulong likesCount)
        {
            if (post != null)
            {
                ID = post.ID;
                Text = post.Text;
                PublishTime = post.PublishTime;
                Image = new FileViewModel(post.Image);
                RatingScore = post.RatingScore;
                Author = new UserViewModel(post.Author);
                Type = post.Type;
                IsPublished = post.IsPublished;
                Coordinates = new CoordinatesViewModel(post.Latitude, post.Longitude);
                IsLiked = isLiked;
                LikesCount = likesCount;
                Comments = comments.ToList();
            }
        }
    }
}
