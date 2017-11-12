# RssFeedParser

[![Build Status](https://travis-ci.org/mhtsbt/RssFeedParser.svg?branch=master)](https://travis-ci.org/mhtsbt/RssFeedParser)
[![NuGet](https://img.shields.io/nuget/v/RssFeedParser.svg)](https://www.nuget.org/packages/RssFeedParser)

Easy to use c# RSS-feed-parser. Just pass in an Url and you will get an object of the feed. Supports an ever increasing list of feed types.

## Example usage

```csharp
var rssFeedParser = new FeedParser();
RssFeed feed = rssFeedParser.ParseFeed("https://matthias.tech/rss");
```
