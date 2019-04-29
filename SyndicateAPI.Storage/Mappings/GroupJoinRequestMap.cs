using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupJoinRequestMap : ClassMap<GroupJoinRequest>
    {
        public GroupJoinRequestMap()
        {
            Table("group_join_requests");
            Id(u => u.ID, "id");

            References(e => e.Group, "id_group");
            References(e => e.User, "id_user");

            Map(u => u.Status, "status").CustomType<GroupJoinRequestStatus>().Not.Nullable();
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
