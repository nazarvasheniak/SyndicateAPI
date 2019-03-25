using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class VehicleCategory : PersistentObject, IDeletableObject
    {
        public virtual string Title { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
