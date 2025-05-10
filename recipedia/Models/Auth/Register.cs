using System.ComponentModel.DataAnnotations;

namespace recipedia.Models.Auth
{
    public class Register
    {
        [EmailAddress]
        public string Email { set; get; }

        public string Password { set; get; }
        public string ProfilePictureUrl { set; get; }
        public string Gender { set; get; }
        public DateTime DateOfBirth { set; get; }
        public String Role { set; get; }
    }
}
