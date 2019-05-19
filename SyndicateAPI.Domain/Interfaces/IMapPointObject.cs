using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Interfaces
{
    public interface IMapPointObject
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        MapPointType MapPointType { get; set; }
    }
}
