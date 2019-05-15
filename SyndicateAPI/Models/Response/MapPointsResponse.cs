using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Response
{
    public class MapPointsResponse : ResponseModel
    {
        public List<MapPointViewModel> Points { get; set; }
        public List<PartnerViewModel> Partners { get; set; }
    }
}
