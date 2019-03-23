using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class VehicleCategoryViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public VehicleCategoryViewModel() { }

        public VehicleCategoryViewModel(VehicleCategory vehicleCategory)
        {
            if (vehicleCategory != null)
            {
                ID = vehicleCategory.ID;
                Title = vehicleCategory.Title;
            }
        }
    }
}
