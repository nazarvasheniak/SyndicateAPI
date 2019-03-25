using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class VehicleService : BaseCrudService<Vehicle>, IVehicleService
    {
        public VehicleService(IRepository<Vehicle> repository) : base(repository)
        {
        }
    }
}
