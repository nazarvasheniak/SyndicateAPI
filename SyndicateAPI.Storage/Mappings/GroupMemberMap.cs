using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupMemberMap : ClassMap<GroupMember>
    {
        public GroupMemberMap()
        {
            Table("group_members");
            Id(u => u.ID, "id");

            References(e => e.Group, "id_group");
            References(e => e.User, "id_user");

            Map(u => u.IsActive, "is_active").Not.Nullable();
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
