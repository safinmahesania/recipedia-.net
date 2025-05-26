using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipedia.Database;
using recipedia.Models;
using recipedia.Models.API;
using System.Security.Claims;

namespace recipedia.Controllers
{
    [Controller]
    [Authorize]
    [Route("api/favorite")]
    public class FavoriteController : ControllerBase
    {
        private readonly DBContext db;
        private readonly UserManager<User> userManager;
        private APIResponse response;

        public FavoriteController(DBContext _context, UserManager<User> _userManager)
        {
            db = _context;
            userManager = _userManager;
            response = new APIResponse();
        }

        [HttpPost("{recipeId}")]
        public async Task<APIResponse> AddToFavorites(int recipeId)
        {

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(userID);

            //var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                response.Message = "User not found";
                response.StatusCode = 401;
                response.IsSuccess = false;

                return response;
            }

            var recipe = await db.Recipe.FindAsync(recipeId);
            if (recipe == null)
            {
                response.StatusCode = 404;
                response.Message = "Recipe not found";
                response.IsSuccess = false;

                return response;

            }

            var exists = await db.Favorite.AnyAsync(f => f.UserId == user.Id && f.RecipeId == recipeId);
            if (exists)
            {
                response.Message = "Already added to favorites";
                response.StatusCode = 400;
                response.IsSuccess = false;

                return response;

            }

            var favorite = new Favorite
            {
                UserId = user.Id,
                RecipeId = recipeId
            };

            db.Favorite.Add(favorite);
            await db.SaveChangesAsync();

            response.StatusCode = 200;
            response.Message = "Recipe added to favorite";
            response.IsSuccess = true;

            return response;
        }

        [HttpDelete("{recipeId}")]
        public async Task<APIResponse> RemoveFromFavorites(int recipeId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                response.Message = "Unauthorized";
                response.StatusCode = 401;
                response.IsSuccess = false;

                return response;
            }

            var recipe = await db.Recipe.FindAsync(recipeId);
            if (recipe == null)
            {
                response.StatusCode = 404;
                response.Message = "Recipe not found";
                response.IsSuccess = false;

                return response;

            }

            var favorite = await db.Favorite.FindAsync(user.Id, recipe.Id);

            if (favorite == null)
            {

                response.StatusCode = 404;
                response.Message = "Recipe not found in favorites";
                response.IsSuccess = false;

                return response;

            }

            db.Favorite.Remove(favorite);
            await db.SaveChangesAsync();

            response.StatusCode = 200;
            response.Message = "Recipe removed from favorite";
            response.IsSuccess = true;

            return response;
        }

        [HttpGet]
        public async Task<APIResponse> GetAllFavoriteRecipes()
        {

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                response.Message = "Unauthorized";
                response.StatusCode = 401;
                response.IsSuccess = false;

                return response;
            }



            var favoriteRecipes = await db.Favorite
                .Where(f => f.UserId == user.Id)
                .Include(f => f.Recipe)
                .Select(f => f.Recipe)
                .ToListAsync();

            if (favoriteRecipes == null)
            {
                response.Message = "No favorite recipes";
                response.StatusCode = 204;
                response.IsSuccess = false;

                return response;
            }

            response.StatusCode = 200;
            response.Message = "Favorite Recipe removed from favorite";
            response.IsSuccess = true;
            response.Data = favoriteRecipes;

            return response;
        }
    }
}
