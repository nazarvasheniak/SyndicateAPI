﻿using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

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
        public VehicleDriveViewModel Drive { get; set; }
        public VehicleTransmissionViewModel Transmission { get; set; }
        public VehicleBodyViewModel Body { get; set; }
        public UserViewModel Owner { get; set; }

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
                Photo = new FileViewModel(vehicle.Photo);
                Class = new VehicleClassViewModel(vehicle.Class);
                Category = new VehicleCategoryViewModel(vehicle.Category);
                Drive = new VehicleDriveViewModel(vehicle.Drive);
                Transmission = new VehicleTransmissionViewModel(vehicle.Transmission);
                Body = new VehicleBodyViewModel(vehicle.Body);
                Owner = new UserViewModel(vehicle.Owner);
            }
        }
    }
}
