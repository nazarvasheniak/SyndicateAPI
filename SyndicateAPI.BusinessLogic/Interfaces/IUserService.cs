using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IUserService : IBaseCrudService<User>
    {
        string AuthorizeUser(string username, string password);
    }
}
