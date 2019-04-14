using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IUserService : IBaseCrudService<User>
    {
        bool IsEmailExist(string email);
        bool IsNicknameExist(string email);
        User CreateUser(string nickname, string password, Person person);
        string AuthorizeUser(string username, string password);
    }
}
