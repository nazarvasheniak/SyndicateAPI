using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string FirsName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public long CityID { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public bool Subscribe { get; set; }
    }
}
