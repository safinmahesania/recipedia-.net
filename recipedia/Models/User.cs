using Microsoft.AspNetCore.Identity;

namespace recipedia.Models
{
    public class User : IdentityUser
    {
        public string ProfilePictureUrl { set; get; }
        public string Gender { set; get; }
        public DateTime DateOfBirth { set; get; }
        public String Role { set; get; }
        public DateTime LastLogin { set; get; }
        public DateTime CreatedAt { set; get; }
        public DateTime UpdatedAt { set; get; }

    }
}
