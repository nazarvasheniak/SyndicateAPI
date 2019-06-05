using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleMap : ClassMap<Vehicle>
    {
        public VehicleMap()
        {
            Table("vehicles");

            Id(u => u.ID, "id");
            
            References(e => e.Category, "id_category");
            References(e => e.Drive, "id_drive");
            References(e => e.Transmission, "id_transmission");
            References(e => e.Body, "id_body");
            References(e => e.Owner, "id_owner");
            References(e => e.ConfirmationPhoto, "id_conf");

            Map(u => u.Model, "model");
            Map(u => u.Power, "power");
            Map(u => u.Year, "year");
            Map(u => u.Price, "price");
            Map(u => u.ApproveStatus, "approve_status").CustomType<VehicleApproveStatus>();
            Map(u => u.ConfirmationText, "conf_text");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
