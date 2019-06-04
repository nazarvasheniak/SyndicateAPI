using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
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

            References(e => e.Dialog, "id_dialog");
            References(e => e.Sender, "id_sender");

            Map(u => u.Type, "message_type").CustomType<DialogMessageType>();
            Map(u => u.Content, "message_content");
            Map(u => u.Time, "message_time");
            Map(u => u.IsReaded, "is_readed");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
