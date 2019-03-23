using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class Person : PersistentObject, IDeletableObject
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Biography { get; set; }
        public virtual City City { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
