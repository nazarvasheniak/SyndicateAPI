using SyndicateAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models
{
    public class VehicleBodyViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public VehicleBodyViewModel() { }

        public VehicleBodyViewModel(VehicleBody drive)
        {
            if (drive != null)
            {
                ID = drive.ID;
                Title = drive.Title;
            }
        }
    }
}
