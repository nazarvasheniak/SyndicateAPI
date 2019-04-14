using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System.Linq;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class PersonService : BaseCrudService<Person>, IPersonService
    {
        public PersonService(IRepository<Person> repository) : base(repository)
        {
        }

        public Person CreatePerson(string firstName, string lastName, string email, City city)
        {
            var person = GetAll()
                .FirstOrDefault(x => x.Email == email);

            if (person != null)
                return null;

            person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                City = city
            };

            Create(person);

            return person;
        }
    }
}
