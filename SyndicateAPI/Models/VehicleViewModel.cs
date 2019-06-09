using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SyndicateAPI.Models
{
    public class VehicleViewModel
    {
        public long ID { get; set; }
        public string Model { get; set; }
        public int Power { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public List<FileViewModel> Photos { get; set; }
        public VehicleCategoryViewModel Category { get; set; }
        public VehicleDriveViewModel Drive { get; set; }
        public VehicleTransmissionViewModel Transmission { get; set; }
        public VehicleBodyViewModel Body { get; set; }
        public UserViewModel Owner { get; set; }
        public FileViewModel ConfirmationPhoto { get; set; }
        public string ConfirmationText { get; set; }
        public VehicleApproveStatus ApproveStatus { get; set; }

        public VehicleViewModel() { }

        public VehicleViewModel(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                ID = vehicle.ID;
                Model = vehicle.Model;
                Power = vehicle.Power;
                Year = vehicle.Year;
                Price = vehicle.Price;
                Category = new VehicleCategoryViewModel(vehicle.Category);
                Drive = new VehicleDriveViewModel(vehicle.Drive);
                Transmission = new VehicleTransmissionViewModel(vehicle.Transmission);
                Body = new VehicleBodyViewModel(vehicle.Body);
                Owner = new UserViewModel(vehicle.Owner);
                ConfirmationPhoto = new FileViewModel(vehicle.ConfirmationPhoto);
                ConfirmationText = vehicle.ConfirmationText;
                ApproveStatus = vehicle.ApproveStatus;
            }
        }

        public VehicleViewModel(Vehicle vehicle, IEnumerable<VehiclePhoto> vehiclePhotos)
        {
            if (vehicle != null)
            {
                ID = vehicle.ID;
                Model = vehicle.Model;
                Power = vehicle.Power;
                Year = vehicle.Year;
                Price = vehicle.Price;
                Category = new VehicleCategoryViewModel(vehicle.Category);
                Drive = new VehicleDriveViewModel(vehicle.Drive);
                Transmission = new VehicleTransmissionViewModel(vehicle.Transmission);
                Body = new VehicleBodyViewModel(vehicle.Body);
                Owner = new UserViewModel(vehicle.Owner);
                ConfirmationPhoto = new FileViewModel(vehicle.ConfirmationPhoto);
                ApproveStatus = vehicle.ApproveStatus;
                ConfirmationText = vehicle.ConfirmationText;
                Photos = vehiclePhotos.Select(x => new FileViewModel(x.Photo)).ToList();
            }
        }
    }
}
