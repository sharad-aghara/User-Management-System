using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UMS.DAL.Interfaces;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "New Password and Confirm Password do not match.");
                    return View(model);
                }

                var userEmail = _userService.GetUserEmailFromToken(HttpContext);

                if (userEmail == null)
                {
                    return Unauthorized();
                }

                // Change the password
                var result = await _userService.ChangePasswordAsync(userEmail, model.NewPassword);

                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Error");
                }
            }

            // Return to the view with validation errors if the model state is invalid
            return View(model);
        }


        private string? GetUserEmailFromToken()
        {
            // Retrieve the JWT token from the Authorization header
            var token = Request.Headers["AuthToken"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userEmailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            return userEmailClaim?.Value;
        }
    }
}
