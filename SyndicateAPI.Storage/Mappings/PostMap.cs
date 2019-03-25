using FluentNHibernate.Mapping;
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
            References(e => e.MinRatingLevel, "id_rating_level");

            Map(u => u.Text, "text");
            Map(u => u.PublishTime, "publish_time");
            Map(u => u.IsPublished, "is_published").Not.Nullable();
            Map(u => u.Type, "type");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
