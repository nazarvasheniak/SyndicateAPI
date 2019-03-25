using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class RatingLevelMap : ClassMap<RatingLevel>
    {
        public RatingLevelMap()
        {
            Table("rating_levels");

            Id(u => u.ID, "id");

            Map(u => u.Title, "title");
            Map(u => u.PointsCount, "points_count");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
