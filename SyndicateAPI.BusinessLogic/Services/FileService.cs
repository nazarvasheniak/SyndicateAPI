using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class FileService : BaseCrudService<File>, IFileService
    {
        public FileService(IRepository<File> repository) : base(repository)
        {
        }
    }
}
