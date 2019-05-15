using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class DriftPlaygroundViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }

        public DriftPlaygroundViewModel() { }

        public DriftPlaygroundViewModel(DriftPlayground playground)
        {
            if (playground != null)
            {
                ID = playground.ID;
                Name = playground.Name;
                Description = playground.Description;
                Coordinates = new CoordinatesViewModel(playground.Latitude, playground.Longitude);
            }
        }
    }
}
