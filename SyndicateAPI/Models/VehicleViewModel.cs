using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class VehicleViewModel
    {
        public long ID { get; set; }
        public string Model { get; set; }
        public int Power { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public FileViewModel Photo { get; set; }
        public VehicleClassViewModel Class { get; set; }
        public VehicleCategoryViewModel Category { get; set; }
        public DriveType Drive { get; set; }
        public TransmissionType Transmission { get; set; }
        public BodyType Body { get; set; }
        public UserViewModel Owner { get; set; }

        public VehicleViewModel() { }

        public VehicleViewModel(Vehicle vehicle)
        {
            ID = vehicle.ID;
            Model = vehicle.Model;
            Power = vehicle.Power;
            Year = vehicle.Year;
            Price = vehicle.Price;
            Photo = new FileViewModel(vehicle.Photo);
            Class = new VehicleClassViewModel(vehicle.Class);
            Category = new VehicleCategoryViewModel(vehicle.Category);
            Drive = vehicle.Drive;
            Transmission = vehicle.Transmission;
            Body = vehicle.Body;
            Owner = new UserViewModel(vehicle.Owner);
        }
    }
}
