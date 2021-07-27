﻿using RssFeedParser.Models;
using System.IO;
using System.Net.Http;
using Xunit;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;

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

            Assert.Equal(feed.Articles.Count, 30);

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

            Assert.Equal(feed.Articles.Count, 40);         

        }

        [Fact]
        public async void TestCNETFeed()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "CNET1.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(feed.Articles.Count, 25);

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

            Assert.Equal(feed.Articles.Count, 99);

            foreach (var article in feed.Articles)
            {
                Assert.False(string.IsNullOrEmpty(article.Image));

            }

        }

        [Fact]
        public async void TestIndieHackers()
        {

            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "Indiehackers.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(feed.Articles.Count, 235);
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

        [Fact]
        public async Task FeedParserExtractsEnclosures()
        {
            var contents = File.ReadAllText(Path.Combine("ExampleFeeds", "PodcastWithAudio.xml"));

            XDocument doc = XDocument.Parse(contents);

            var rssFeedParser = new FeedParser();
            RssFeed feed = await rssFeedParser.ParseFeed(doc);

            Assert.Equal(2, feed.Articles.Count);
            
            // each article should contain at least one enclosure
            foreach(var article in feed.Articles)
            {
                Assert.Equal(1, article.Enclosures.Count);

                var enclosure = article.Enclosures[0];
                Assert.Equal("audio/mpeg", enclosure.Type);
                Assert.Equal(1024, enclosure.Length);
                Assert.StartsWith("https://example.org/episodes/download/", enclosure.Url);
            }

        }

    }
}
