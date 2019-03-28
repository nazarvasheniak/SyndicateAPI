using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("users");

            Id(u => u.ID, "id");

            References(e => e.Avatar, "id_avatar");
            References(e => e.Person, "id_person");
            References(e => e.Group, "id_group");

            Map(u => u.Login, "login").Unique();
            Map(u => u.Password, "password");
            Map(u => u.Nickname, "nickname").Unique();
            Map(u => u.PointsCount, "points_coint");
            Map(u => u.RegTime, "reg_time");
            Map(u => u.ActivationCode, "activation_code");
            Map(u => u.ActivationTime, "activation_time");
            Map(u => u.IsActive, "is_active").Not.Nullable();
            Map(u => u.IsOnline, "is_online").Not.Nullable();
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
