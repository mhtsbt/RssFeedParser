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
        public async void TestMashableFeed()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "Mashable1.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(30, feed.Articles.Count);

            foreach (var article in feed.Articles)
            {
                Assert.NotNull(article.Image);

                var path = new Uri(article.Image);
                Assert.True(Path.HasExtension(path.AbsoluteUri));
            }

        }

        [Fact]
        public async void TestTweakersFeed()
        {
            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "Tweakers1.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(40, feed.Articles.Count);         
        }

        [Fact]
        public async void TestCNETFeed()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "CNET1.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(25, feed.Articles.Count);

            foreach (var article in feed.Articles)
            {
                Assert.False(string.IsNullOrEmpty(article.Image));

                var path = new Uri(article.Image);
                Assert.True(Path.HasExtension(path.AbsoluteUri));
            }

        }

        [Fact]
        public async void TestGuardianFeed()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "Guardian1.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(99, feed.Articles.Count);

            foreach (var article in feed.Articles)
                Assert.False(string.IsNullOrEmpty(article.Image));
        }

        [Fact]
        public async void TestIndieHackers()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "Indiehackers.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(235, feed.Articles.Count);
        }

        [Fact]
        public void FeedParserShouldParseArticles()
        {
            //var feed = "http://feeds.feedburner.com/ChefSteps";

            //var rssFeedParser = new FeedParser();
            //var articles = rssFeedParser.ParseFeed(feed);


            //string feedUrl = "http://uk.businessinsider.com/rss";
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
