using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PostLikeMap : ClassMap<PostLike>
    {
        public PostLikeMap()
        {
            Table("post_likes");
            Id(u => u.ID, "id");

            References(e => e.Post, "id_post");
            References(e => e.User, "id_user");

            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
