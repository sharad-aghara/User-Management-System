using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UMS.Core.Interface;
using UMS.DAL.Interfaces;
using UMS.DAL.Models;
using UMS.DAL.Services;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAdminServices _adminService;
        private readonly IEmailService _emailService;

        private readonly BaseRepository<User> _userRepo;

        public AdminController(IUserService userService, IAdminServices adminServices, IEmailService emailService, BaseRepository<User> userRepo)
        {
            _userService = userService;
            _adminService = adminServices;
            _emailService = emailService;

            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var unapprovedUsers = await _userService.GetUnapprovedUsersAsync();

            var userViewModels = unapprovedUsers.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                PhotoPath = user.PhotoPath,
                StateId = user.StateId,
                DistrictId = user.DistrictId,
                TalukaId = user.TalukaId,
                RoleId = user.RoleId,
                IsPasswordChanged = user.IsPasswordChanged,
                DateOfBirth = user.DateOfBirth,
                //IsApproved = user.IsApproved
                //Hobbies = user.Hobbies,
            }).ToList();

            // Pass the view models to the view
            return View(userViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var result = await _adminService.ApproveUser(id);
            

            if (result)
            {
                var user = await _userRepo.GetByIdAsync(id);

                Console.WriteLine("user.Email");

                await _emailService.NotifyUserProfileApproved(user.Email, user.PasswordHash);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> RejectUser(int id)
        {
            var result = await _adminService.RejectUser(id);

            if (result)
            {
                var user = await _userRepo.GetByIdAsync(id);

                await _emailService.NotifyUserProfileNotApproved(user.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
