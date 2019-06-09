using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class UserSessionMap : ClassMap<UserSession>
    {
        public UserSessionMap()
        {
            Table("user_sessions");
            Id(u => u.ID, "id");

            References(e => e.User, "id_user");

            Map(u => u.StartDate, "start_date");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
