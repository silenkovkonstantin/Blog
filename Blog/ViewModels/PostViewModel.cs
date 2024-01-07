using Blog.Data.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public User User { get; set; }
        public string UserId { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<Comment> Comments { get; set; }
    }
}