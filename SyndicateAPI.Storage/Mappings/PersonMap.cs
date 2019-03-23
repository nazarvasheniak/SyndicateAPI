using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("persons");

            Id(u => u.ID, "id");

            References(e => e.City, "id_city");

            Map(u => u.FirstName, "first_name");
            Map(u => u.LastName, "last_name");
            Map(u => u.Email, "email");
            Map(u => u.Biography, "bio");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
