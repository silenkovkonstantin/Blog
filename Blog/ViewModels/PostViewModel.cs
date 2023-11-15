using Blog.Data.Models.Db;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
