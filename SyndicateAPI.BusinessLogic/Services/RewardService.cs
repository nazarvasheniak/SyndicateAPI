using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class RewardService : BaseCrudService<Reward>, IRewardService
    {
        public RewardService(IRepository<Reward> repository) : base(repository)
        {
        }
    }
}
