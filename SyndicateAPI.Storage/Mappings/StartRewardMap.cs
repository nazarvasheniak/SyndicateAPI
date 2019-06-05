using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class StartRewardMap : ClassMap<StartReward>
    {
        public StartRewardMap()
        {
            Table("start_rewards");
            Id(u => u.ID, "id");

            References(e => e.User, "id_user");

            Map(u => u.IsAvatarCompleted, "is_avatar_completed");
            Map(u => u.IsBiographyCompleted, "is_biography_completed");
            Map(u => u.IsVehicleCompleted, "is_vehicle_completed");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
