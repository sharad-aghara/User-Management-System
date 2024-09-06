using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace WebApplication2.ViewModels
{
    public class DistrictViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int StateId { get; set; } // Dropdown to select a state
        public IEnumerable<SelectListItem> States { get; set; } // List of states for dropdown
    }

}
