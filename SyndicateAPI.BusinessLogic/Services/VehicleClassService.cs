using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class VehicleClassService : BaseCrudService<VehicleClass>, IVehicleClassService
    {
        public VehicleClassService(IRepository<VehicleClass> repository) : base(repository)
        {
        }
    }
}
