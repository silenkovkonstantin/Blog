using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogAPI.Contracts.Models.Tags.GetTagsResponse;

namespace BlogAPI.Contracts.Models.Posts
{
    public class AddPostRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public List<TagView> Tags { get; set; }
    }
}
