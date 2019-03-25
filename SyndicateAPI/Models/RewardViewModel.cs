using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Models
{
    public class RewardViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FileViewModel Icon { get; set; }

        public RewardViewModel() { }

        public RewardViewModel(Reward reward)
        {
            if (reward != null)
            {
                ID = reward.ID;
                Name = reward.Name;
                Description = reward.Description;
                Icon = new FileViewModel(reward.Icon);
            }
        }
    }
}
