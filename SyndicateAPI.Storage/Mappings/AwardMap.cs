using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class AwardMap : ClassMap<Award>
    {
        public AwardMap()
        {
            Table("awards");
            Id(u => u.ID, "id");

            References(e => e.Awarder, "id_awarder");
            References(e => e.Rewarder, "id_rewarder");
            References(e => e.Reward, "id_reward");

            Map(u => u.Comment, "comment");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
