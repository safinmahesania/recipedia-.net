using Microsoft.AspNetCore.Identity;

namespace recipedia.Models
{
    public class Favorite
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
