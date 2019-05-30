using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class MapPointMap : ClassMap<MapPoint>
    {
        public MapPointMap()
        {
            Table("map_points");
            Id(x => x.ID, "id");

            References(e => e.User, "id_user");

            Map(u => u.Name, "name");
            Map(u => u.Message, "message");
            Map(u => u.Latitude, "latitude");
            Map(u => u.Longitude, "longitude");
            Map(u => u.MapPointType, "map_point_type").CustomType<MapPointType>();
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
