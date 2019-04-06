using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Models
{
    public class CoordinatesViewModel
    {
        private double _latitude { get; set; }
        private double _longitude { get; set; }

        public CoordinatesViewModel(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public double Latitude
        {
            get => _latitude;
        }

        public double Longitude
        {
            get => _longitude;
        }
    }
}
