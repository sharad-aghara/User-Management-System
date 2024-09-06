using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class LoginViewModel
    {
        [Display(Prompt = "Your Email")]
        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress(ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }

        [Display(Prompt = "Your Password")]
        [Required(ErrorMessage = "Please enter your Password")]
        public string Password { get; set; }
    }
}
