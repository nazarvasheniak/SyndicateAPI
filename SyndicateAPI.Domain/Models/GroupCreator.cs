
using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class GroupCreator : PersistentObject, IDeletableObject
    {
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
