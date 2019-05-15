using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class AwardViewModel
    {
        public long ID { get; set; }
        public UserViewModel Awarder { get; set; }
        public UserViewModel Rewarder { get; set; }
        public RewardViewModel Reward { get; set; }
        public string Comment { get; set; }

        public AwardViewModel() { }

        public AwardViewModel(Award award)
        {
            if (award != null)
            {
                ID = award.ID;
                Awarder = new UserViewModel(award.Awarder);
                Rewarder = new UserViewModel(award.Rewarder);
                Reward = new RewardViewModel(award.Reward);
                Comment = award.Comment;
            }
        }
    }
}
