using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleBodyMap : ClassMap<VehicleBody>
    {
        public VehicleBodyMap()
        {
            Table("vehicle_bodies");

            Id(u => u.ID, "id");

            Map(u => u.Title, "title");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
