using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class GroupPostViewModel
    {
        public long ID { get; set; }
        public PostViewModel Post { get; set; }

        public GroupPostViewModel() { }

        public GroupPostViewModel(GroupPost groupPost)
        {
            if (groupPost != null)
            {
                ID = groupPost.ID;
                Post = new PostViewModel(groupPost.Post);
            }
        }
    }
}
