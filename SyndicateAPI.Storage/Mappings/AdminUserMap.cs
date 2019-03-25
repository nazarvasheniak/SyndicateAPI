using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class AdminUserMap : ClassMap<AdminUser>
    {
        public AdminUserMap()
        {
            Table("admin_users");

            Id(u => u.ID, "id");

            References(e => e.Person, "id_person");

            Map(u => u.Login, "login");
            Map(u => u.Password, "password");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
