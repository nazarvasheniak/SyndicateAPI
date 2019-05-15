using System;
using System.Collections.Generic;
using System.Text;

namespace SyndicateAPI.Domain.Interfaces
{
    public interface IMapPointObject
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}
