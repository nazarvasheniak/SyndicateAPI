using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class DialogMap : ClassMap<Dialog>
    {
        public DialogMap()
        {
            Table("dialogs");
            Id(u => u.ID, "id");

            References(e => e.FromUser, "id_from");
            References(e => e.ToUser, "id_to");

            Map(u => u.StartDate, "start_date");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
