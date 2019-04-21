using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PostCommentLikeMap : ClassMap<PostCommentLike>
    {
        public PostCommentLikeMap()
        {
            Table("post_comment_likes");
            Id(u => u.ID, "id");

            References(e => e.Comment, "id_comment");
            References(e => e.User, "id_user");

            Map(u => u.Deleted, "deleted");
        }
    }
}
