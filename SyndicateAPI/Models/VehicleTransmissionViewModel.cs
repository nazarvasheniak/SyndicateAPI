using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class VehicleTransmissionViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public VehicleTransmissionViewModel() { }

        public VehicleTransmissionViewModel(VehicleTransmission drive)
        {
            if (drive != null)
            {
                ID = drive.ID;
                Title = drive.Title;
            }
        }
    }
}
