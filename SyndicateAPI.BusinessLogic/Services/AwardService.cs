using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class AwardService : BaseCrudService<Award>, IAwardService
    {
        public AwardService(IRepository<Award> repository) : base(repository)
        {
        }
    }
}
