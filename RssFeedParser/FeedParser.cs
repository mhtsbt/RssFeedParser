using HtmlAgilityPack;
using RssFeedParser.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RssFeedParser
{

    public class FeedParser
    {

        public async Task<RssFeed> ParseFeed(string url)
        {
            using HttpClient client = new HttpClient();
            Stream stream = await client.GetStreamAsync(url);
            return await ParseFeed(XDocument.Load(stream));
        }

        public async Task<RssFeed> ParseFeed(XDocument doc)
        {
            var outputFeed = new RssFeed();
            var type = DetermineFeedType(doc);

            XElement root = doc.Root;

            if (type is FeedTypes.RSS)
            {
                var items = root.Element("channel").Elements("item");

                if (items != null)
                    foreach (XElement item in items)
                        outputFeed.Articles.Add(ParseRSSArticle(item));
            }
            else
            {
                var items = doc.Elements().First().Elements().Where(e => e.Name.LocalName == "entry");

                if (items != null)
                    foreach (XElement item in items)
                        outputFeed.Articles.Add(ParseAtomArticle(item));
            }

            try
            {

                using var client = new HttpClient();

                foreach (var article in outputFeed.Articles.Where(x => string.IsNullOrEmpty(x.Image)))
                    article.Image = await UseOgTagForArticleImage(client, article);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to get additional images");
            }

            return outputFeed;

        }

        private async Task<string> UseOgTagForArticleImage(HttpClient client, RssFeedArticle article)
        {
            var articleResponse = await client.GetAsync(article.Link);
            var html = await articleResponse.Content.ReadAsStringAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // If no og-image found just return;
            var elements = htmlDocument.DocumentNode.SelectNodes("//meta[contains(@property, 'og:image')]");
            if (elements == null || !elements.Any()) return string.Empty;

            // If no content for tag found return;
            var ogImageMetaTag = elements.Where(x => x.Attributes.Any(a => a.Value == "og:image")).First();
            var ogImageContentTag = ogImageMetaTag.GetAttributeValue("content", null);
            if (string.IsNullOrWhiteSpace(ogImageContentTag)) return string.Empty;

            // If not a valid url just return null
            return !Uri.TryCreate(ogImageContentTag, UriKind.RelativeOrAbsolute, out Uri mediaUrl) ? string.Empty : mediaUrl.ToString();
        }

        private FeedTypes DetermineFeedType(XDocument doc)
        {
            XElement root = doc.Root;

            return root.Element("channel") != null ? FeedTypes.RSS : FeedTypes.Atom;
        }

        private RssFeedArticle ParseRSSArticle(XElement item)
        {
            var newArticle = new RssFeedArticle();

            if (item.Element("title") != null)
                newArticle.Title = item.Element("title").Value;

            XNamespace dc = "http://search.yahoo.com/mrss/";

            if (item.Element(dc + "thumbnail") != null && item.Element(dc + "thumbnail").Attribute("url") != null)
                newArticle.Image = item.Element(dc + "thumbnail").Attribute("url").Value;

            if (item.Element("description") != null)
            {
                newArticle.Content = item.Element("description").Value;

                if (string.IsNullOrEmpty(newArticle.Image))
                    newArticle.Image = FindThumbnailForArticle(newArticle.Content);
            }

            if (item.Element("link") != null)
                newArticle.Link = item.Element("link").Value;

            if (item.Element("pubDate") != null)
                newArticle.Published = ParsePublishedDate(item.Element("pubDate").Value);

            if (item.Elements("category") != null)
                foreach (var cat in item.Elements("category"))
                    newArticle.Categories.Add(new RssFeedArticleCategory()
                    {
                        Name = cat.Value.ToLower()
                    });

            return newArticle;
        }

        private RssFeedArticle ParseAtomArticle(XElement item)
        {
            var newArticle = new RssFeedArticle();

            var elements = item.Elements();

            if (elements.First(i => i.Name.LocalName == "title") != null)
            newArticle.Title = item.Elements().First(i => i.Name.LocalName == "title").Value;

            if (elements.FirstOrDefault(i => i.Name != null && i.Name.LocalName == "content") != null)
            {
                newArticle.Content = item.Elements().First(i => i.Name.LocalName == "content").Value;
                newArticle.Image = FindThumbnailForArticle(newArticle.Content);
            }

            if (elements.FirstOrDefault(i => i.Name.LocalName == "link") != null && item.Elements().First(i => i.Name.LocalName == "link").Attribute("href") != null)
                newArticle.Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value;

            if (elements.FirstOrDefault(i => i.Name.LocalName == "published") != null)
                newArticle.Published = ParsePublishedDate(item.Elements().First(i => i.Name.LocalName == "published").Value);

            return newArticle;
        }

        private DateTime ParsePublishedDate(string dateString) => DateTime.Parse(dateString);

        private string FindThumbnailForArticle(string description)
        {
            Regex regex = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.:()?=%&=\-@/$,]*");
            Match match = regex.Match(description);

            var imageUrl = match.Value;

            return Path.HasExtension(imageUrl) ? imageUrl : string.Empty;
        }
    }
}
