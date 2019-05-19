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

            References(e => e.Participant1, "id_participant1");
            References(e => e.Participant2, "id_participant2");

            Map(u => u.StartDate, "start_date");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
