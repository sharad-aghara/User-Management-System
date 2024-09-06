using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.DAL.DTO;
using UMS.DAL.Models;

namespace UMS.DAL.Interfaces
{
    public interface IUserService
    {
        Task addUser(User user);
        Task<User> InitUserAsync(User entity, int Id);
        Task<List<User>> GetUsersAsync();
        Task<User> FindUserAndLogin(string email, string password);
        Task<User> FindUserByEmail(string email);
        Task<List<District>> GetDistrictsByStateIdAsync(int stateId);
        Task<List<Taluka>> GetTalukasByDistrictIdAsync(int districtId);
        Task<List<User>> GetUnapprovedUsersAsync();
        Task<bool> ChangePasswordAsync(string email, string password);
        string GetUserEmailFromToken(HttpContext httpContext);
    }
}
