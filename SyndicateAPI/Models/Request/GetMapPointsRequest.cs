using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class GetMapPointsRequest
    {
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public double Radius { get; set; }
    }
}
