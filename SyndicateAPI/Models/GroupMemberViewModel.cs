using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class GroupMemberViewModel
    {
        public long ID { get; set; }
        public UserViewModel User { get; set; }
        public RoleInGroup Role { get; set; }

        public GroupMemberViewModel() { }

        public GroupMemberViewModel(GroupMember groupMember)
        {
            ID = groupMember.ID;
            User = new UserViewModel(groupMember.User);
            Role = RoleInGroup.Member;
        }

        public GroupMemberViewModel(GroupMember groupMember, RoleInGroup role)
        {
            ID = groupMember.ID;
            User = new UserViewModel(groupMember.User);
            Role = role;
        }
    }
}
