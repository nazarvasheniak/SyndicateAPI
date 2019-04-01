
namespace SyndicateAPI.Models.Request
{
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public long CityID { get; set; }
        public long AvatarID { get; set; }
        public string Biography { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
