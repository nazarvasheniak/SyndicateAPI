using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Models
{
    public class VehicleClassViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public FileViewModel Icon { get; set; }

        public VehicleClassViewModel() { }

        public VehicleClassViewModel(VehicleClass vehicleClass)
        {
            ID = vehicleClass.ID;
            Title = vehicleClass.Title;
            Icon = new FileViewModel(vehicleClass.Icon);
        }
    }
}
