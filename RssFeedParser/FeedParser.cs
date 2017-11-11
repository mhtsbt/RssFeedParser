using RssFeedParser.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RssFeedParser
{

    public class FeedParser
    {

        public RssFeed ParseFeed(string url)
        {
            HttpClient client = new HttpClient();
            Stream stream = client.GetStreamAsync(url).Result;
            return ParseFeed(XDocument.Load(stream));
        }

        public RssFeed ParseFeed(XDocument doc)
        {
            var outputFeed = new RssFeed();
            var type = DetermineFeedType(doc);

            XElement root = doc.Root;

            if (type == FeedTypes.RSS)
            {
                var items = root.Element("channel").Elements("item");

                if (items != null)
                {
                    foreach (XElement item in items)
                    {
                        outputFeed.Articles.Add(ParseRSSArticle(item));
                    }
                }
            }
            else
            {
                var items = doc.Elements().First().Elements().Where(e => e.Name.LocalName == "entry");

                if (items != null)
                {
                    foreach (XElement item in items)
                    {
                        outputFeed.Articles.Add(ParseAtomArticle(item));
                    }
                }
            }

            return outputFeed;

        }

        private FeedTypes DetermineFeedType(XDocument doc)
        {
            XElement root = doc.Root;

            if (root.Element("channel") != null)
            {
                return FeedTypes.RSS;
            }
            else
            {
                return FeedTypes.Atom;
            }

        }

        private RssFeedArticle ParseRSSArticle(XElement item)
        {
            var newArticle = new RssFeedArticle();

            if (item.Element("title") != null)
            {
                newArticle.Title = item.Element("title").Value;
            }

            /* TODO (for cnet articles for example)
             *   if (item.Elements().First(i => i.Name.LocalName == "media:thumbnail") != null)
            {
                newArticle.Image = item.Elements().First(i => i.Name.LocalName == "media:thumbnail").Value;
            }
            else
            */

            XNamespace dc = "http://search.yahoo.com/mrss/";

            if (item.Element(dc + "thumbnail") != null && item.Element(dc + "thumbnail").Attribute("url") != null)
            {
                newArticle.Image = item.Element(dc + "thumbnail").Attribute("url").Value;
            }

            if (item.Element("description") != null)
            {

                newArticle.Content = item.Element("description").Value;

                if (string.IsNullOrEmpty(newArticle.Image))
                {
                    newArticle.Image = FindThumbnailForArticle(newArticle.Content);
                }

            }

            if (item.Element("link") != null)
            {
                newArticle.Link = item.Element("link").Value;
            }

            if (item.Element("pubDate") != null)
            {
                newArticle.Published = ParsePublishedDate(item.Element("pubDate").Value);
            }

            if (item.Elements("category") != null)
            {
                foreach (var cat in item.Elements("category"))
                {
                    newArticle.Categories.Add(new RssFeedArticleCategory()
                    {
                        Name = cat.Value.ToLower()
                    });
                }

            }

            return newArticle;
        }

        private RssFeedArticle ParseAtomArticle(XElement item)
        {
            var newArticle = new RssFeedArticle();

            if (item.Elements().First(i => i.Name.LocalName == "title") != null)
            {
                newArticle.Title = item.Elements().First(i => i.Name.LocalName == "title").Value;
            }

            if (item.Elements().First(i => i.Name.LocalName == "content") != null)
            {
                newArticle.Content = item.Elements().First(i => i.Name.LocalName == "content").Value;
                newArticle.Image = FindThumbnailForArticle(newArticle.Content);
            }

            if (item.Elements().First(i => i.Name.LocalName == "link") != null && item.Elements().First(i => i.Name.LocalName == "link").Attribute("href") != null)
            {
                newArticle.Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value;
            }

            if (item.Elements().First(i => i.Name.LocalName == "published") != null)
            {
                newArticle.Published = ParsePublishedDate(item.Elements().First(i => i.Name.LocalName == "published").Value);
            }

            return newArticle;
        }

        private DateTime ParsePublishedDate(string dateString)
        {
            return DateTime.Parse(dateString);
        }

        private string FindThumbnailForArticle(string description)
        {
            Regex regex = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.:()?=%&=\-@/$,]*");
            Match match = regex.Match(description);

            return match.Value;

        }

    }
}
