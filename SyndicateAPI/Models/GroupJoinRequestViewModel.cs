using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class GroupJoinRequestViewModel
    {
        public long ID { get; set; }
        public UserViewModel User { get; set; }
        public GroupJoinRequestStatus Status { get; set; }

        public GroupJoinRequestViewModel() { }

        public GroupJoinRequestViewModel(GroupJoinRequest joinRequest)
        {
            if (joinRequest != null)
            {
                ID = joinRequest.ID;
                Status = joinRequest.Status;
                User = new UserViewModel(joinRequest.User);
            }
        }
    }
}
