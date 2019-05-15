using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class RewardMap : ClassMap<Reward>
    {
        public RewardMap()
        {
            Table("rewards");

            Id(u => u.ID, "id");

            References(e => e.Icon, "id_icon");
            References(e => e.User, "id_user");

            Map(u => u.Name, "name");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
