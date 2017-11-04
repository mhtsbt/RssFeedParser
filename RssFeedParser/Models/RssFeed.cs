using System.Collections.Generic;

namespace RssFeedParser.Models
{
    public class RssFeed
    {

        public RssFeed()
        {
            Articles = new List<RssFeedArticle>();
        }

        public List<RssFeedArticle> Articles { get; set; }
    }
}
