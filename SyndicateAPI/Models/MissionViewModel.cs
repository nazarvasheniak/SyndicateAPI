using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class MissionViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }

        public MissionViewModel() { }

        public MissionViewModel(Mission mission)
        {
            if (mission != null)
            {
                ID = mission.ID;
                Name = mission.Name;
                Description = mission.Description;
                Coordinates = new CoordinatesViewModel(mission.Latitude, mission.Longitude);
            }
        }
    }
}
