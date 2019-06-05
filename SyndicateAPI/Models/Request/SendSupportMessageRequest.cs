using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Models.Request
{
    public class SendSupportMessageRequest
    {
        public string Email { get; set; }
        
        public string Name { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
