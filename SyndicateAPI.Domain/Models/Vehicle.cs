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
        public virtual File Photo { get; set; }
        public virtual VehicleClass Class { get; set; }
        public virtual VehicleCategory Category { get; set; }
        public virtual DriveType Drive { get; set; }
        public virtual TransmissionType Transmission { get; set; }
        public virtual BodyType Body { get; set; }
        public virtual User Owner { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
