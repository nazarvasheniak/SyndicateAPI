using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class VehicleProperties
    {
        public List<VehicleCategoryViewModel> Categories { get; set; }
        public List<VehicleDriveViewModel> Drives { get; set; }
        public List<VehicleTransmissionViewModel> Transmissions { get; set; }
        public List<VehicleBodyViewModel> Bodies { get; set; }
    }
}
