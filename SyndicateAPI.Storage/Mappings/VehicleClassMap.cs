using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleClassMap : ClassMap<VehicleClass>
    {
        public VehicleClassMap()
        {
            Table("vehicle_classes");

            Id(u => u.ID, "id");

            References(e => e.Icon, "id_icon");

            Map(u => u.Title, "title");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
