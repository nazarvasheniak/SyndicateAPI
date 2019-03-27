using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleTransmissionMap : ClassMap<VehicleTransmission>
    {
        public VehicleTransmissionMap()
        {
            Table("vehicle_transmissions");

            Id(u => u.ID, "id");

            Map(u => u.Title, "title");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
