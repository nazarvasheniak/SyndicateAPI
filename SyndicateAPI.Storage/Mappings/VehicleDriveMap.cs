using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleDriveMap : ClassMap<VehicleDrive>
    {
        public VehicleDriveMap()
        {
            Table("vehicle_drives");

            Id(u => u.ID, "id");

            Map(u => u.Title, "title");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
