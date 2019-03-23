using FluentNHibernate.Mapping;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Storage.Mappings
{
    public class FileMap : ClassMap<File>
    {
        public FileMap()
        {
            Table("files");

            Id(u => u.ID, "id");

            Map(u => u.Name, "name");
            Map(u => u.Type, "type").CustomType<FileType>();
            Map(u => u.Url, "url");
            Map(u => u.LocalPath, "local_path");
            Map(u => u.Deleted, "deleted").Not.Nullable();
        }
    }
}
