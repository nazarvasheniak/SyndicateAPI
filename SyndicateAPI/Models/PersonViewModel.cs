using SyndicateAPI.Domain.Models;

namespace SyndicateAPI.Models
{
    public class PersonViewModel
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public CityViewModel City { get; set; }

        public PersonViewModel() { }

        public PersonViewModel(Person person)
        {
            if (person != null)
            {
                ID = person.ID;
                FirstName = person.FirstName;
                LastName = person.LastName;
                Email = person.Email;
                Biography = person.Biography;
                City = new CityViewModel(person.City);
            }
        }
    }
}
