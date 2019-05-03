using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SyndicateAPI.Models
{
    public class GroupViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string FullDesc { get; set; }
        public string Information { get; set; }
        public FileViewModel Avatar { get; set; }
        public UserViewModel Owner { get; set; }
        public RoleInGroup Role { get; set; }
        public List<GroupPostViewModel> Posts { get; set; }
        public List<UserViewModel> Subscribers { get; set; }
        public List<GroupMemberViewModel> Members { get; set; }
        public List<GroupJoinRequestViewModel> JoinRequests { get; set; }

        public GroupViewModel() { }

        public GroupViewModel(Group group)
        {
            if (group != null)
            {
                ID = group.ID;
                Name = group.Name;
                ShortDesc = group.ShortDesc;
                FullDesc = group.FullDesc;
                Information = group.Information;
                Avatar = new FileViewModel(group.Avatar);
                Owner = new UserViewModel(group.Owner);
                Posts = new List<GroupPostViewModel>();
                Subscribers = new List<UserViewModel>();
                Members = new List<GroupMemberViewModel>();
                JoinRequests = new List<GroupJoinRequestViewModel>();
            }
        }

        public GroupViewModel(Group group, IEnumerable<GroupPostViewModel> posts, IEnumerable<UserViewModel> subscribers, IEnumerable<GroupMemberViewModel> members, RoleInGroup role, IEnumerable<GroupJoinRequestViewModel> joinRequests)
        {
            if (group != null)
            {
                ID = group.ID;
                Name = group.Name;
                ShortDesc = group.ShortDesc;
                FullDesc = group.FullDesc;
                Information = group.Information;
                Role = role;
                Avatar = new FileViewModel(group.Avatar);
                Owner = new UserViewModel(group.Owner);
                Subscribers = subscribers.ToList();
                Members = members.ToList();
                JoinRequests = joinRequests.ToList();
            }

            Posts = posts
                .Where(x => x.Post.IsPublished)
                .ToList();
        }
    }
}
