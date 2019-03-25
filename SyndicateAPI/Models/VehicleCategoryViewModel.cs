using SyndicateAPI.Domain.Models;

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
