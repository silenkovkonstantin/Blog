namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public UserViewModel User { get; set; }
    }
}
