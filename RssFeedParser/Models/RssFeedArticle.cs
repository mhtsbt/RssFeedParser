using System;
using System.Collections.Generic;

namespace RssFeedParser.Models
{
    public class RssFeedArticle
    {

        public RssFeedArticle()
        {
            Categories = new List<RssFeedArticleCategory>();
        }

        public string Title { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime Published { get; set; }
        public List<RssFeedArticleCategory> Categories { get; set; }

    }
}