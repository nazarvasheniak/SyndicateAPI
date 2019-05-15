using SyndicateAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class CreatePartnerRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public long MapIconID { get; set; }
        public long LogoID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public MapPointType MapPointType { get; set; }
    }
}
