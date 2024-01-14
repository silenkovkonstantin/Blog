using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Contracts.Models.Tags
{
    public class GetTagsResponse
    {
        public int TagAmount { get; set; }
        public List<TagView> Tags { get; set; }

        public class TagView
        {
            public string Name { get; set; }
            public bool IsChecked { get; set; }
        }
    }
}
