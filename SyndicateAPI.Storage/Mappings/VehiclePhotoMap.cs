using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehiclePhotoMap : ClassMap<VehiclePhoto>
    {
        public VehiclePhotoMap()
        {
            Table("vehicle_photos");

            Id(u => u.ID, "id");

            References(e => e.Photo, "id_photo");
            References(e => e.Vehicle, "id_vehicle");

            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
