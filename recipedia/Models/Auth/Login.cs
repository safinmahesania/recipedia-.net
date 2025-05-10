using System.ComponentModel.DataAnnotations;

namespace recipedia.Models.Auth
{
    public class Login
    {
        [EmailAddress]
        public string Email { set; get; }
        public string Password { set; get; }
    }
}
