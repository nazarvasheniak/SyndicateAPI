using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupMap : ClassMap<Group>
    {
        public GroupMap()
        {
            Table("groups");

            Id(u => u.ID, "id");

            Map(u => u.Name, "name");
            Map(u => u.ShortDesc, "short_desc");
            Map(u => u.FullDesc, "full_desc");
            Map(u => u.Avatar, "id_avatar");
            Map(u => u.Owner, "id_owner");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
