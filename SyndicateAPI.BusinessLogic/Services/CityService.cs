using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class CityService : BaseCrudService<City>, ICityService
    {
        public CityService(IRepository<City> repository) : base(repository)
        {
        }
    }
}
