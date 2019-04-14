using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IPersonService : IBaseCrudService<Person>
    {
        Person CreatePerson(string firstName, string lastName, string email, City city);
    }
}
