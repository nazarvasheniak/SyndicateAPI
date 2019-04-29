using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PointsRewardMap : ClassMap<PointsReward>
    {
        public PointsRewardMap()
        {
            Table("points_rewards");
            Id(u => u.ID, "id");

            References(e => e.User, "id_user");

            Map(u => u.PointsCount, "points_count");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
