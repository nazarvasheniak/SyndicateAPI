using SyndicateAPI.Domain.Interfaces;

namespace SyndicateAPI.Domain.Models
{
    public class PointsReward : PersistentObject, IDeletableObject
    {
        public virtual long PointsCount { get; set; }
        public virtual User User { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
