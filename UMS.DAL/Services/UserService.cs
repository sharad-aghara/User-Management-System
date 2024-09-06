using HarfBuzzSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Interface;
using UMS.DAL.DTO;
using UMS.DAL.Interfaces;
using UMS.DAL.Models;

namespace UMS.DAL.Services
{
    public class UserService : IUserService
    {
        private readonly BaseRepository<User> _userRepo;
        private readonly ApplicationDbContext _dbContext;

        public UserService(BaseRepository<User> userRepo, ApplicationDbContext Context)
        {
            _userRepo = userRepo;
            _dbContext = Context;
        }

        public async Task addUser(User user)
        {
            await _userRepo.AddAsync(user);
        }

        public async Task<User> FindUserAndLogin(string email, string password)
        {
            var allUsers = await _userRepo.ListAllAsync();
            var user = allUsers.Where(user => user.Email == email && user.PasswordHash == password).FirstOrDefault();
            return user;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var allUsers = await _userRepo.ListAllAsync();
            var user = allUsers.Where(user => user.Email == email).FirstOrDefault();
            return user;
        }

        public Task<List<User>> GetUsersAsync()
        {
            var entities = _userRepo.ListAllAsync();
            return entities;
        }

        // On Hold
        public Task<User> InitUserAsync(User entity, int Id)
        {
            throw new NotImplementedException();
        }
        //
 
        public async Task<List<District>> GetDistrictsByStateIdAsync(int stateId)
        {
            return await _dbContext.Districts
                .Where(e => e.StateId == stateId)
                .ToListAsync();
        }

        public async Task<List<Taluka>> GetTalukasByDistrictIdAsync(int districtId)
        {
            return await _dbContext.Talukas
                .Where(e => e.DistrictId == districtId)
                .ToListAsync();
        }


        public async Task<List<User>> GetUnapprovedUsersAsync()
        {
            return await _dbContext.Users.Where(u => u.IsApproved == false).ToListAsync();
        }

        public async Task<bool> ChangePasswordAsync(string email, string newPassword)
        {
            var user = await FindUserByEmail(email); 

            if (user != null)
            {
                user.PasswordHash = newPassword;
                //user.PasswordHash = HashPassword(newPassword);
                await _userRepo.UpdateAsync(user);
                return true;
            }
            return false;
        }

        public string GetUserEmailFromToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            return emailClaim?.Value;
        }

        //private string HashPassword(string password)
        //{
        //    return BCrypt.Net.BCrypt.HashPassword(password);
        //}

    }
}
