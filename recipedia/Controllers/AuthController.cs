using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipedia.Database;
using recipedia.Models;
using recipedia.Models.API;
using recipedia.Models.Auth;
using System.Net;

namespace recipedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly DBContext db;
        private readonly APIResponse response;


        public AuthController(UserManager<User> _userManager, SignInManager<User> _signInManager, DBContext _db)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
            response = new APIResponse();
        }

        private async Task<string> GenerateUniqueUserNameAsync(string baseName)
        {
            string name = baseName;
            int count = 1;

            while (await userManager.FindByNameAsync(name) != null)
            {
                name = $"{baseName}{count}";
                count++;
            }

            return name;
        }

        [HttpPost("register")]
        public async Task<APIResponse> Register([FromBody] Register newUser) {

            if (!ModelState.IsValid)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = "Bad Request";

                return response;
            }


            var user = new User
            {
                UserName = await GenerateUniqueUserNameAsync(newUser.Email.Split('@')[0]),
                Email = newUser.Email,
                ProfilePictureUrl = newUser.ProfilePictureUrl,
                Gender = newUser.Gender,
                Role = newUser.Role,
                DateOfBirth = newUser.DateOfBirth,
                LockoutEnabled = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var result = await userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded)
            {
                response.Errors = result.Errors;
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = "Bad Request";

                return response;
            }


            //if (await roleManager.RoleExistsAsync(newUser.Role))
            //    await userManager.AddToRoleAsync(user, newUser.Role);

            response.StatusCode = 200;
            response.Message = "User registered successfully.";
            response.IsSuccess = true;
            response.Data = user;
            return response;
        }

        [HttpPost("login")]
        public async Task<APIResponse> Login([FromBody] Login loginCred)
        {

            if (!ModelState.IsValid) {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = "Bad Request";

                return response;
            }

            var user = await userManager.FindByEmailAsync(loginCred.Email);
            if (user == null)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = "Email doesn't exists.";

                return response;
            }

            var result = await signInManager.PasswordSignInAsync(user.UserName!, loginCred.Password, false, false);

            if (!result.Succeeded)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Message = "Error occurred, please try again later.";

                return response;
            }
            
            //var user = await userManager.FindByNameAsync(loginCred.Username);

            response.StatusCode = 200;
            response.Message = "Logged in successfully.";
            response.IsSuccess = true;
            response.Data = user;
            return response;
        }

        [HttpPost("forget-password")]
        public async Task<APIResponse> ForgetPassword([FromBody] ForgetPassword userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail.Email);

            if (user == null)
            {
                response.StatusCode = 204;
                response.Message = "User not found.";
                response.IsSuccess = false;

                return response;
            }


            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            token = WebUtility.UrlEncode(token);

            // 👉 Option 2: Also return token to mobile app directly (less secure unless authenticated)

            response.StatusCode = 200;
            response.Message = "User found.";
            response.IsSuccess = true;
            response.Data = new {
                token = token,
                user = user
            }; 

            return response;
        }

        [HttpPost("reset-password")]
        public async Task<APIResponse> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            var user = await userManager.FindByEmailAsync(resetPassword.Id);
            var token = WebUtility.UrlDecode(resetPassword.Token);
            var result = await userManager.ResetPasswordAsync(user!,token, resetPassword.NewPassword);

            if (!result.Succeeded)
            {
                response.StatusCode = 400;
                response.Message = "Error Occurred.";
                response.IsSuccess = false;
                response.Errors = result.Errors;

                return response;
            }

            user = await userManager.FindByIdAsync(resetPassword.Id);
            response.StatusCode = 200;
            response.Message = "Password reset successfully.";
            response.IsSuccess = true;
            response.Data = user;

            return response;
        }
    }
}
