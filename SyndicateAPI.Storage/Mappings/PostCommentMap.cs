using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class PostCommentMap : ClassMap<PostComment>
    {
        public PostCommentMap()
        {
            Table("post_comment");
            Id(u => u.ID, "id");

            References(e => e.Post, "id_post");
            References(e => e.Author, "id_author");

            Map(u => u.Text, "comment_text");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
