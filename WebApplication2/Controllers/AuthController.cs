using Microsoft.AspNetCore.Mvc;
using UMS.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using UMS.BL.Helpers;
using WebApplication2.ViewModels;
using UMS.Core;

namespace WebApplication2.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user, string ReturnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var hashedPassword = HashedPasswordHelper.HashPassword(user.Password);

            //var foundUser = await _userService.FindUserAndLogin(user.Email, user.Password);
            var foundUser = await _userService.FindUserByEmail(user.Email);

            var validUser = HashedPasswordHelper.VerifyPassword(user.Password, foundUser.PasswordHash);

            var userEntity = await _userService.FindUserByEmail(user.Email);

            if (validUser && userEntity != null)
            {
                var token = _configuration.GenerateToken(foundUser);

                CookieHelper.SetCookie(HttpContext, Constant.JWT_COOKIE_NAME, token, Constant.DEFAULT_COOKIE_EXPIRY);

                if (ReturnUrl != null)
                {
                    var segments = ReturnUrl.Split('/');
                    //return RedirectToRoute(ReturnUrl);
                    return RedirectToAction(segments[segments.Length - 2], segments[segments.Length - 3], new { id = segments[segments.Length - 1] });
                }

                bool isPasswordChanged = (bool)userEntity.IsPasswordChanged;

                if(isPasswordChanged) {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account");
                }

                
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            CookieHelper.RemoveCookie(HttpContext, Constant.JWT_COOKIE_NAME);
            return RedirectToAction("Login", "Auth");
        }
    }
}
