using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.DAL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string PhotoPath { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public string Hobbies { get; set; } // Can be a comma-separated string or JSON
        public DateOnly DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public string PasswordHash { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
