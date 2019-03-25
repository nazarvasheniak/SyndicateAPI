﻿using SyndicateAPI.Domain.Enums;
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
        public long ClassID { get; set; }

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
        public DriveType Drive { get; set; }

        [Required]
        public TransmissionType Transmission { get; set; }

        [Required]
        public BodyType Body { get; set; }
    }
}