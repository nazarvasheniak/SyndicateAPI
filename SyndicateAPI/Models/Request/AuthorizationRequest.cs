using System.ComponentModel.DataAnnotations;

namespace SyndicateAPI.Models.Request
{
    public class AuthorizationRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
