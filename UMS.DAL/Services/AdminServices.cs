using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.DAL.Interfaces;
using UMS.DAL.Models;

namespace UMS.DAL.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly BaseRepository<User> _userRepo;

        public AdminServices(BaseRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<bool> ApproveUser(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                user.IsApproved = true;
                await _userRepo.UpdateAsync(user);
                return true;
            }

            return false;
        }

        public async Task<bool> RejectUser(int id) 
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                user.IsApproved = false;
                await _userRepo.UpdateAsync(user);
                return true;
            }

            return false;
        }
    }
}
