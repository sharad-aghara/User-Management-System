using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class StateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

}
