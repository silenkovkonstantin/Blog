using Blog.Data.Models.Db;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public UserViewModel User { get; set; }
        public string UserId { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
