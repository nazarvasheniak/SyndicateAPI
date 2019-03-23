using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class CityViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }

        public CityViewModel() { }

        public CityViewModel(City city)
        {
            if (city != null)
            {
                ID = city.ID;
                Name = city.Name;
            }
        }
    }
}
