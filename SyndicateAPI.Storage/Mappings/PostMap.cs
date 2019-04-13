using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PostMap : ClassMap<Post>
    {
        public PostMap()
        {
            Table("posts");

            Id(u => u.ID, "id");

            References(e => e.Author, "id_author");
            References(e => e.Image, "id_image");

            Map(u => u.Text, "text");
            Map(u => u.PublishTime, "publish_time");
            Map(u => u.IsPublished, "is_published").Not.Nullable();
            Map(u => u.RatingScore, "rating_score");
            Map(u => u.Type, "type").CustomType<PostType>();
            Map(u => u.Latitude, "latitude");
            Map(u => u.Longitude, "longtitude");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
