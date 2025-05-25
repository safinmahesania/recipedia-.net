using Azure;
using Microsoft.AspNetCore.Mvc;
using recipedia.Database;
using recipedia.Models;
using recipedia.Models.API;

namespace recipedia.Controllers
{
    [ApiController]
    [Route("api/recipe")]
    public class RecipeController
    {
        private readonly DBContext db;
        private APIResponse response;
        public RecipeController(DBContext _db)
        {
            response = new APIResponse();
            db = _db;
        }

        [HttpGet]
        public async Task<APIResponse> GetAllRecipes()
        {
            try
            {
                List<Recipe> data = db.recipe.ToList();
                if (data == null)
                {
                    response.StatusCode = 204;
                    response.IsSuccess = true;
                    response.Message = "No Content";
                }
                else
                {
                    response.Data = data;
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Message = "Recipe fetched successfully";
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<APIResponse> GetRecipeByID(int Id)
        {
            try
            {
                Recipe data = db.recipe.First(u => u.Id == Id);
                if (data == null)
                {
                    response.StatusCode = 204;
                    response.IsSuccess = true;
                    response.Message = "No Content";
                }
                else
                {
                    response.Data = data;
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Message = "Recipe fetched successfully";
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = e.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public async Task<APIResponse> GetRecipeByID(string name)
        {
            try
            {
                Recipe data = db.recipe.First(u => u.Name == name);
                if (data == null)
                {
                    response.StatusCode = 204;
                    response.IsSuccess = true;
                    response.Message = "No Content";
                }
                else
                {
                    response.Data = data;
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Message = "Recipe fetched successfully";
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = e.Message;
            }
            return response;
        }

        [HttpPost]
        public async Task<APIResponse> AddRecipe([FromBody] Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    response.StatusCode = 400;
                    response.IsSuccess = false;
                    response.Message = "Invalid recipe data";
                }
                else
                {
                    db.recipe.Add(recipe);
                    db.SaveChanges();

                    response.Data = recipe;
                    response.StatusCode = 201;
                    response.IsSuccess = true;
                    response.Message = "Recipe created successfully";
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return response;
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<APIResponse> DeleteRecipe(int id)
        {
            try
            {
                bool exists = db.recipe.Any(u => u.Id == id);
                if (!exists)
                {
                    response.StatusCode = 204;
                    response.IsSuccess = false;
                    response.Message = "Recipe ID does not exists";
                }
                else
                {
                    Recipe data = db.recipe.First(u => u.Id == id);

                    db.recipe.Remove(data);
                    db.SaveChanges();

                    response.Data = data;
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Message = "Recipe successfully deleted";
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
