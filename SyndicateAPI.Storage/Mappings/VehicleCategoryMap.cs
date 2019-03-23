using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class VehicleCategoryMap : ClassMap<VehicleCategory>
    {
        public VehicleCategoryMap()
        {
            Table("vehicle_categories");

            Id(u => u.ID, "id");

            Map(u => u.Title, "title");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
