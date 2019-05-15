using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class OffroadTrackViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }

        public OffroadTrackViewModel() { }

        public OffroadTrackViewModel(OffroadTrack track)
        {
            if (track != null)
            {
                ID = track.ID;
                Name = track.Name;
                Description = track.Description;
                Coordinates = new CoordinatesViewModel(track.Latitude, track.Longitude);
            }
        }
    }
}
