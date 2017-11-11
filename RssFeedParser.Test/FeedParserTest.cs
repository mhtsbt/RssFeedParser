using RssFeedParser.Models;
using System.IO;
using System.Net.Http;
using Xunit;
using System;
using System.Xml;
using System.Xml.Linq;

namespace RssFeedParser.Test
{
    public class FeedParserTest
    {
        public FeedParserTest()
        {
        }

        [Fact]
        public void TestMashableFeed()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "Mashable1.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = rssFeedParser.ParseFeed(doc);

            Assert.Equal(feed.Articles.Count, 30);

            foreach (var article in feed.Articles)
            {
                Assert.NotNull(article.Image);

                var path = new Uri(article.Image);
                Assert.True(Path.HasExtension(path.AbsoluteUri));
            }

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
