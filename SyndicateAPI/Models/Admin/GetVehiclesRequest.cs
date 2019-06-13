using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Models.Request;

namespace SyndicateAPI.Models.Admin
{
    public class GetVehiclesRequest : GetListRequest
    {
        public VehicleApproveStatus Status { get; set; }
    }
}
