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

            Map(u => u.Name, "name");
            Map(u => u.Description, "description");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
