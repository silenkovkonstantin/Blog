using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.Db
{
    public class Comment
    {
        /// <summary>
        /// Модель комментария
        /// </summary>
        public int Id { get; set; }
        // Внешний ключ
        public int PostId { get; set; }
        // Навигационное свойство
        public Post Post { get; set; }

        [Required (ErrorMessage = "Не задан текст комментария")]
        [StringLength (500, MinimumLength = 3, ErrorMessage = "Длина текста должна быть от 3 до 500 символов")]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        // Внешний ключ
        public string UserId { get; set; }
        // Навигационное свойство
        public User User { get; set; }
    }
}
