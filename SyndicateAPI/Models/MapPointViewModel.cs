using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class MapPointViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }
        public MapPointType MapPointType { get; set; }
        public UserViewModel User { get; set; }

        public MapPointViewModel() { }

        public MapPointViewModel(MapPoint point)
        {
            if (point != null)
            {
                ID = point.ID;
                Name = point.Name;
                Message = point.Message;
                Coordinates = new CoordinatesViewModel(point.Latitude, point.Longitude);
                MapPointType = point.MapPointType;
                User = new UserViewModel(point.User);
            }
        }
    }
}
