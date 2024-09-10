using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.BL.Helpers;
using UMS.DAL.Interfaces;
using UMS.DAL.Models;

namespace UMS.DAL.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly BaseRepository<User> _userRepo;
        //private readonly EmailService _emailService;

        public AdminServices(BaseRepository<User> userRepo)
        {
            _userRepo = userRepo;

            //_emailService = emailService;
        }

        public async Task<bool> ApproveUserAndGeneratePassword(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user != null)
            {
                //var password = HashedPasswordHelper.GenerateRandomPassword();
                //var hashedPassword = HashedPassword.HashPassword(password);

                //user.PasswordHash = hashedPassword;
                user.IsApproved = true;
                await _userRepo.UpdateAsync(user);
                //await _emailService.NotifyUserProfileApproved(user.Email, password);

                return true;
            }

            return false;
        }

        public async Task<bool> RejectUser(int id) 
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                //user.IsApproved = false;
                //await _userRepo.DeleteAsync(user);
                //await _userRepo.UpdateAsync(user);
                //await _emailService.NotifyUserProfileNotApproved(user.Email);
                return true;
            }

            return false;
        }
    }
}
