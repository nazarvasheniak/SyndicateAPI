using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupModeratorMap : ClassMap<GroupModerator>
    {
        public GroupModeratorMap()
        {
            Table("group_moderators");
            Id(u => u.ID, "id");

            References(e => e.Group, "id_group");
            References(e => e.User, "id_user");

            Map(u => u.Level, "moderator_level").CustomType<GroupModeratorLevel>().Not.Nullable();
            Map(u => u.IsActive, "is_active").Not.Nullable();
        }
    }
}
