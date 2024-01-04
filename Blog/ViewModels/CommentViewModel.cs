using Blog.Data.Models.Db;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public User User { get; set; }
    }
}
