using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class UserTempMap : ClassMap<UserTemp>
    {
        public UserTempMap()
        {
            Table("users_temp");

            Id(u => u.ID, "id");

            References(e => e.User, "id_user");

            Map(u => u.Email, "email");
            Map(u => u.TempCode, "temp_code");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
