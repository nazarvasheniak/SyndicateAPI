using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class CreateVehicleResponse : DataResponse<VehicleViewModel>
    {
        public long BonusPoints { get; set; } = 0;
    }
}
