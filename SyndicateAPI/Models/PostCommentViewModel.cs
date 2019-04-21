using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class PostCommentViewModel
    {
        public long ID { get; set; }
        public string Text { get; set; }
        public bool IsLiked { get; set; }
        public ulong LikesCount { get; set; }
        public UserViewModel Author { get; set; }

        public PostCommentViewModel() { }

        public PostCommentViewModel(PostComment comment, bool isLiked, ulong likesCount)
        {
            if (comment != null)
            {
                ID = comment.ID;
                Text = comment.Text;
                IsLiked = isLiked;
                LikesCount = likesCount;
                Author = new UserViewModel(comment.Author);
            }
        }
    }
}
