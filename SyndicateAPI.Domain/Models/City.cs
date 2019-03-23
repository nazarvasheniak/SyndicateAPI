using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class City : PersistentObject, IDeletableObject
    {
        public virtual string Name { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
