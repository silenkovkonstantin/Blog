using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
