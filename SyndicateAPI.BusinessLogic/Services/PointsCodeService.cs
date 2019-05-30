using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class PointsCodeService : BaseCrudService<PointsCode>, IPointsCodeService
    {
        public PointsCodeService(IRepository<PointsCode> repository) : base(repository)
        {
        }
    }
}
