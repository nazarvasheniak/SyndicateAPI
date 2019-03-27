using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class VehicleTransmissionService : BaseCrudService<VehicleTransmission>, IVehicleTransmissionService
    {
        public VehicleTransmissionService(IRepository<VehicleTransmission> repository) : base(repository)
        {
        }
    }
}
