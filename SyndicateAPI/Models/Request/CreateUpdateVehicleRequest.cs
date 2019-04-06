using SyndicateAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class CreateUpdateVehicleRequest
    {
        [Required]
        public long CategoryID { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Power { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public long PhotoID { get; set; }

        [Required]
        public long DriveID { get; set; }

        [Required]
        public long TransmissionID { get; set; }

        [Required]
        public long BodyID { get; set; }

        [Required]
        public long ConfirmationPhotoID { get; set; }
    }
}
