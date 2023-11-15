using Blog.Data.Models.Db;

namespace Blog.ViewModels
{
    public class TagsViewModel
    {
        public List<Tag> Tags { get; set; }
        public TagViewModel NewTag { get; set; }

        public TagsViewModel()
        {
            NewTag = new TagViewModel();
        }
    }
}
