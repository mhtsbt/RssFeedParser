using RssFeedParser.Models;
using System.IO;
using System.Net.Http;
using Xunit;

namespace RssFeedParser.Test
{
    public class FeedParserTest
    {
        public FeedParserTest()
        {
        }

        [Fact]
        public void FeedParserShouldParseArticles()
        {

            var feed = "http://feeds.feedburner.com/ChefSteps";

            var rssFeedParser = new FeedParser();
            var articles = rssFeedParser.ParseFeed(feed);


            string feedUrl = "http://uk.businessinsider.com/rss";
            //  string feedUrl = "http://coffeegeek.com/rss";

            //var parser = new FeedParser();

            //HttpClient client = new HttpClient();
            //Stream stream = client.GetStreamAsync(feedUrl).Result;

            //// XElement root = XDocument.Load(stream).Root;
            //// var items = root.Element("channel").Elements("item");

            //RssFeed feed = parser.ParseFeed(feedUrl);

            //Assert.Equal(feed.Articles.Count, items.Count());
        }

    }
}
