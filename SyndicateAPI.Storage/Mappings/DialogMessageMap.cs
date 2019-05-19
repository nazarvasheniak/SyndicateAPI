using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Storage.Mappings
{
    public class DialogMessageMap : ClassMap<DialogMessage>
    {
        public DialogMessageMap()
        {
            Table("dialog_messages");
            Id(u => u.ID, "id");

            References(e => e.Sender, "id_sender");
            References(e => e.Dialog, "id_dialog");

            Map(u => u.Content, "message_content");
            Map(u => u.Time, "message_time");
            Map(u => u.IsReaded, "is_readed");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
