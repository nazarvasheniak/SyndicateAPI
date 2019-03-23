using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleMap : ClassMap<Vehicle>
    {
        public VehicleMap()
        {
            Table("vehicles");

            Id(u => u.ID, "id");

            References(e => e.Photo, "id_photo");
            References(e => e.Class, "id_class");
            References(e => e.Category, "id_category");
            References(e => e.Owner, "id_owner");

            Map(u => u.Model, "model");
            Map(u => u.Power, "power");
            Map(u => u.Year, "year");
            Map(u => u.Price, "price");
            Map(u => u.Drive, "drive").CustomType<DriveType>();
            Map(u => u.Transmission, "transmission").CustomType<TransmissionType>();
            Map(u => u.Body, "body").CustomType<BodyType>();
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
