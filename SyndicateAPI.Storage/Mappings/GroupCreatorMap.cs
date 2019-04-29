using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupCreatorMap : ClassMap<GroupCreator>
    {
        public GroupCreatorMap()
        {
            Table("group_creators");
            Id(u => u.ID, "id");

            References(e => e.Group, "id_group");
            References(e => e.User, "id_user");

            Map(u => u.IsActive, "is_active").Not.Nullable();
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
