using System.ComponentModel.DataAnnotations;

namespace SyndicateAPI.Models.Request
{
    public class ActivationRequest
    {
        [Required]
        public long UserID { get; set; }

        [Required]
        public int Code { get; set; }
    }
}
