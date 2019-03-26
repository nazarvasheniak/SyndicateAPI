using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class GroupPostMap : ClassMap<GroupPost>
    {
        public GroupPostMap()
        {
            Table("group_posts");

            Id(u => u.ID, "id");

            References(e => e.Post, "id_post");
            References(e => e.Group, "id_group");

            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
