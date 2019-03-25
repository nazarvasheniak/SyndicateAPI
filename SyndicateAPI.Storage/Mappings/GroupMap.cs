using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupMap : ClassMap<Group>
    {
        public GroupMap()
        {
            Table("groups");

            Id(u => u.ID, "id");

            References(e => e.Avatar, "id_avatar");
            References(e => e.Owner, "id_owner");

            Map(u => u.Name, "name");
            Map(u => u.ShortDesc, "short_desc");
            Map(u => u.FullDesc, "full_desc");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
