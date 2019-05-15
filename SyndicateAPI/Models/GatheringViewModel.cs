using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class GatheringViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }

        public GatheringViewModel() { }

        public GatheringViewModel(Gathering gathering)
        {
            if (gathering != null)
            {
                ID = gathering.ID;
                Name = gathering.Name;
                Description = gathering.Description;
                Coordinates = new CoordinatesViewModel(gathering.Latitude, gathering.Longitude);
            }
        }
    }
}
