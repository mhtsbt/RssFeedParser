/*
using System;
using Reader.Business;
using Sherpa.Networking;
using System.IO;

namespace RssFeedParser.Test.Services
{
    public class FavIconGrabberBuilder
    {
        private IHttpService _httpService;

        public FavIconGrabberBuilder WithHttpService(IHttpService httpService)
        {
            _httpService = httpService;
            return this;
        }

        public FavIconGrabber Build()
        {
            return new FavIconGrabber(_httpService, null);
        }

        public string GetExampleHTML()
        {
            return File.ReadAllText(Path.Combine("Data", "examplehtml.html"));
        }
    }
}*/