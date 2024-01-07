using Blog.Data.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int PostId { get; set; }

        public User User { get; set; }
    }
}
