using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.Db
{
    public class Post
    {
        /// <summary>
        /// Модель поста в блоге
        /// </summary>
        public int Id { get; set; }
        [Required (ErrorMessage = "Не указан заголовок")]
        [StringLength (50, MinimumLength = 3, ErrorMessage = "Длина заголовка должна быть от 3 до 50 символов")]
        public string Title { get; set; }
        [Required (ErrorMessage = "Не указано описание")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Длина описания должна быть от 3 до 50 символов")]
        public string Description { get; set; }
        [Required (ErrorMessage = "Не задан текст статьи")]
        [StringLength(5000, MinimumLength = 3, ErrorMessage = "Длина текста должна быть от 3 до 50 символов")]
        public string Text { get; set; }
        public User Author { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
