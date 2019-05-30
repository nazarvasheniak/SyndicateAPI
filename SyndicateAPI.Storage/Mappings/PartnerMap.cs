using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PartnerMap : ClassMap<Partner>
    {
        public PartnerMap()
        {
            Table("partners");
            Id(u => u.ID, "id");

            References(e => e.Logo, "id_logo");
            References(e => e.MapIcon, "id_icon");
            References(e => e.Creator, "id_creator");

            Map(u => u.Name, "name");
            Map(u => u.Description, "description");
            Map(u => u.MapPointType, "type").CustomType<MapPointType>();
            Map(u => u.Latitude, "latitude");
            Map(u => u.Longitude, "longitude");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
