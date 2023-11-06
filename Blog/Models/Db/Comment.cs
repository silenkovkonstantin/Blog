namespace Blog.Models.Db
{
    public class Comment
    {
        /// <summary>
        /// Модель комментария
        /// </summary>
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public User Author { get; set; }
    }
}
