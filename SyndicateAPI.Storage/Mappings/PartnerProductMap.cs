using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PartnerProductMap : ClassMap<PartnerProduct>
    {
        public PartnerProductMap()
        {
            Table("partner_products");
            Id(u => u.ID, "id");

            References(e => e.Partner, "id_partner");

            Map(u => u.Name, "name");
            Map(u => u.Price, "price");
            Map(u => u.PointsCount, "points_count");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
