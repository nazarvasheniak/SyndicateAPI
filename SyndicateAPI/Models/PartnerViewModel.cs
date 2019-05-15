﻿using SyndicateAPI.Domain.Enums;
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
        public PartnerType Type { get; set; }
        public FileViewModel Logo { get; set; }
        public CoordinatesViewModel Coordinates { get; set; }

        public PartnerViewModel() { }

        public PartnerViewModel(Partner partner)
        {
            if (partner != null)
            {
                ID = partner.ID;
                Name = partner.Name;
                Description = partner.Description;
                Type = partner.Type;
                Logo = new FileViewModel(partner.Logo);
                Coordinates = new CoordinatesViewModel(partner.Latitude, partner.Longitude);
            }
        }
    }
}
