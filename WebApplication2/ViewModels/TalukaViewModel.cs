using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class TalukaViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int DistrictId { get; set; } // Dropdown to select a district
        public IEnumerable<SelectListItem> Districts { get; set; } // List of districts for dropdown
    }

}
