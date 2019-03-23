using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            ID = person.ID;
            FirstName = person.LastName;
            LastName = person.LastName;
            Email = person.Email;
            Biography = person.Biography;
            City = new CityViewModel(person.City);
        }
    }
}
