using Moq;
using RssFeedParser.Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RssFeedParser.Test
{
    /*
    public class FavIconGrabberTest
    {
        [Fact]
        public void GetFavIconURL_WithAvalidURL_ShouldReturnFavIconURL()
        {

            var favIconGrabber = new FavIconGrabberBuilder();

            var mock = new Mock<IHttpService>();
            mock.Setup(http => http.GetURL(It.IsAny<string>())).Returns(favIconGrabber.GetExampleHTML());

            var service = favIconGrabber.WithHttpService(mock.Object).Build();

            var result = service.GetFavIconURL("https://www.geek.com");

            Assert.Equal(result, "https://www.geek.com/wp-content/themes/geek7/favicon.ico");
        }

    }*/
}
