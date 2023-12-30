using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.Db
{
    public class Tag
    {
        /// <summary>
        /// Модель тэга
        /// </summary>
        public int Id { get; set; }
        [Required (ErrorMessage = "Не задано наименование")]
        public string Name { get; set; }
        // Навигационное свойство
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
