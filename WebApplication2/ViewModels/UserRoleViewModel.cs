using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.ViewModels
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; } // Dropdown list of users
        public IEnumerable<SelectListItem> Roles { get; set; } // Dropdown list of roles
    }

}
