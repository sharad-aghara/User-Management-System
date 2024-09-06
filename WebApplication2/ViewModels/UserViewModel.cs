using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; } // Radio button

        public IFormFile? ProfilePhoto { get; set; } // To handle file upload for profile photo

        public string? PhotoPath { get; set; } // Stores the path to the uploaded file

        [Required]
        public int StateId { get; set; } // Dropdown

        [Required]
        public int DistrictId { get; set; } // Dropdown (dynamically populated based on State)

        [Required]
        public int TalukaId { get; set; } // Dropdown (dynamically populated based on District)

        public List<int>? SelectedHobbies { get; set; } // Multiselect checkbox

        [Required]
        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [Required]
        public int RoleId { get; set; } = 2; // Dropdown for roles

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        public bool IsApproved { get; set; }
        public bool? IsPasswordChanged { get; set; }

        // To populate dropdowns
        public IEnumerable<SelectListItem>? States { get; set; }
        public IEnumerable<SelectListItem>? Districts { get; set; }
        public IEnumerable<SelectListItem>? Talukas { get; set; }
        public IEnumerable<SelectListItem>? Hobbies { get; set; } // List of hobbies from DB
        public IEnumerable<SelectListItem>? Roles { get; set; } // List of roles (Admin, User, etc.)
    }

}
