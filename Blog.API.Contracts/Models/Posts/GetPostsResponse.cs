using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogAPI.Contracts.Models.Tags.GetTagsResponse;

namespace BlogAPI.Contracts.Models.Posts
{
    public class GetPostsResponse
    {
        public int PostAmount { get; set; }
        public List<PostView> Posts { get; set; }
    }

    public class PostView
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TagView> Tags { get; set; }
    }
}

