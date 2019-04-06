using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class VehiclePhotoViewModel
    {
        public long ID { get; set; }
        public FileViewModel Photo { get; set; }
        public VehicleViewModel Vehicle { get; set; }

        public VehiclePhotoViewModel() { }

        public VehiclePhotoViewModel(VehiclePhoto vehiclePhoto)
        {
            ID = vehiclePhoto.ID;
            Photo = new FileViewModel(vehiclePhoto.Photo);
            Vehicle = new VehicleViewModel(vehiclePhoto.Vehicle);
        }
    }
}
