using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class DriftPlaygroundMap : ClassMap<DriftPlayground>
    {
        public DriftPlaygroundMap()
        {
            Table("drift_playgrounds");
            Id(u => u.ID, "id");

            Map(u => u.Name, "name");
            Map(u => u.Description, "decription");
            Map(u => u.Latitude, "latitude");
            Map(u => u.Longitude, "longitude");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
