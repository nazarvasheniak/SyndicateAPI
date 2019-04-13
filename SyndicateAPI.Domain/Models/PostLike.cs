using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class PostLike : PersistentObject, IDeletableObject
    {
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
