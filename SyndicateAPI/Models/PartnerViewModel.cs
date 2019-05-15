using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Interfaces;
using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class PartnerViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MapPointType MapPointType { get; set; }
        public FileViewModel Logo { get; set; }
        public FileViewModel MapIcon { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }

        public PartnerViewModel() { }

        public PartnerViewModel(Partner partner)
        {
            if (partner != null)
            {
                ID = partner.ID;
                Name = partner.Name;
                Description = partner.Description;
                MapPointType = partner.MapPointType;
                Logo = new FileViewModel(partner.Logo);
                MapIcon = new FileViewModel(partner.MapIcon);
                Coordinates = new CoordinatesViewModel(partner.Latitude, partner.Longitude);
            }
        }
    }
}
