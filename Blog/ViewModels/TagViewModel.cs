using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}
