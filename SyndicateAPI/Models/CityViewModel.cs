using SyndicateAPI.Domain.Models;

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
