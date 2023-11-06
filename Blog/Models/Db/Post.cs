namespace Blog.Models.Db
{
    public class Post
    {
        /// <summary>
        /// Модель поста в блоге
        /// </summary>
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
