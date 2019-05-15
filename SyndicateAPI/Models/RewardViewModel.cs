using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Models
{
    public class RewardViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public FileViewModel Icon { get; set; }
        public UserViewModel User { get; set; }

        public RewardViewModel() { }

        public RewardViewModel(Reward reward)
        {
            if (reward != null)
            {
                ID = reward.ID;
                Name = reward.Name;
                Icon = new FileViewModel(reward.Icon);
                User = new UserViewModel(reward.User);
            }
        }
    }
}
