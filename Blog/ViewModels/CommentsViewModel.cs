using Blog.Models.Db;

namespace Blog.ViewModels
{
    public class CommentsViewModel
    {
        public User PostAuthor { get; set; }
        public User CommentAuthor { get; set; }
        public List<Comment> Comments { get; set; }
        public CommentViewModel NewComment { get; set; }

        public CommentsViewModel()
        {
            NewComment = new CommentViewModel();
        }
    }
}
