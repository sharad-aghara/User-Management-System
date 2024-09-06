using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }
    }

}
