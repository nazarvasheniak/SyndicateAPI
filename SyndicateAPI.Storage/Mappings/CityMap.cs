using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class CityMap : ClassMap<City>
    {
        public CityMap()
        {
            Table("cities");

            Id(u => u.ID, "id");

            Map(u => u.Name, "name");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
