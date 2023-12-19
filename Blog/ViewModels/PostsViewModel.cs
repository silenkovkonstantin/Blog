using Blog.Data.Models.Db;

namespace Blog.ViewModels
{
    public class PostsViewModel
    {
        public List<Post> Posts { get; set; }
        public PostViewModel NewPost { get; set; }
        public TagsViewModel Tags { get; set; }
        public PostsViewModel()
        {
            NewPost = new PostViewModel();
            Tags = new TagsViewModel();
        }
    }
}
