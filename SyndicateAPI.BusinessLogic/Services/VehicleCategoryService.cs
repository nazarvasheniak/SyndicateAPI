using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class VehicleCategoryService : BaseCrudService<VehicleCategory>, IVehicleCategoryService
    {
        public VehicleCategoryService(IRepository<VehicleCategory> repository) : base(repository)
        {
        }
    }
}
