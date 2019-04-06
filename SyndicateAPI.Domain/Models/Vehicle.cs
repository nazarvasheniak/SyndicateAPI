using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class Vehicle : PersistentObject, IDeletableObject
    {
        public virtual string Model { get; set; }
        public virtual int Power { get; set; }
        public virtual int Year { get; set; }
        public virtual decimal Price { get; set; }
        public virtual VehicleCategory Category { get; set; }
        public virtual VehicleDrive Drive { get; set; }
        public virtual VehicleTransmission Transmission { get; set; }
        public virtual VehicleBody Body { get; set; }
        public virtual User Owner { get; set; }
        public virtual File ConfirmationPhoto { get; set; }
        public virtual VehicleApproveStatus ApproveStatus { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
