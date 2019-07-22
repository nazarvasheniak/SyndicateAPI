using SyndicateAPI.Domain.Enums;
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
        public GroupViewModel Group { get; set; }

        public GroupPostViewModel() { }

        public GroupPostViewModel(GroupPost groupPost)
        {
            if (groupPost != null)
            {
                ID = groupPost.ID;
                Post = new PostViewModel(groupPost.Post);
                Group = new GroupViewModel(groupPost.Group);
            }
        }

        public GroupPostViewModel(GroupPost groupPost, IEnumerable<UserViewModel> subscribers, IEnumerable<GroupMemberViewModel> members, RoleInGroup role, IEnumerable<GroupJoinRequestViewModel> joinRequests)
        {
            ID = groupPost.ID;
            Post = new PostViewModel(groupPost.Post);
            Group = new GroupViewModel(groupPost.Group, new List<GroupPostViewModel>(), subscribers, members, role, joinRequests);
        }

        public GroupPostViewModel(GroupPost groupPost, IEnumerable<UserViewModel> subscribers, IEnumerable<GroupMemberViewModel> members, RoleInGroup role, IEnumerable<GroupJoinRequestViewModel> joinRequests, IEnumerable<GroupPostViewModel> posts)
        {
            ID = groupPost.ID;
            Post = new PostViewModel(groupPost.Post);
            Group = new GroupViewModel(groupPost.Group, posts, subscribers, members, role, joinRequests);
        }
    }
}
