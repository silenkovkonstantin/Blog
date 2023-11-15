using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.Db
{
    public class Comment
    {
        /// <summary>
        /// Модель комментария
        /// </summary>
        public int Id { get; set; }
        public int PostId { get; set; }
        [Required (ErrorMessage = "Не задан текст комментария")]
        [StringLength (500, MinimumLength = 3, ErrorMessage = "Длина текста должна быть от 3 до 500 символов")]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public User Author { get; set; }
    }
}
