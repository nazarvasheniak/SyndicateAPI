using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PointsCodeMap : ClassMap<PointsCode>
    {
        public PointsCodeMap()
        {
            Table("points_codes");
            Id(u => u.ID, "id");

            References(e => e.Creator, "id_creator");

            Map(u => u.Code, "code");
            Map(u => u.PointsCount, "points_count");
            Map(u => u.CreationDate, "creation_date");
            Map(u => u.ExpiresDate, "expires_date");
            Map(u => u.IsUsed, "is_used");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
