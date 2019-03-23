using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class GroupService : BaseCrudService<Group>, IGroupService
    {
        public GroupService(IRepository<Group> repository) : base(repository)
        {
        }
    }
}
