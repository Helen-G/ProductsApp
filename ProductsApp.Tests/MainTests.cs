using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ProductsApp.Tests
{
    [TestFixture]
    public class MainTests
    {
        [Test]
        public void GetAllProducts_returns_200_code()
        {
            //get the base service url from the App.config
            var serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];

            //create an instance of Uri class to validate the service url
            var serviceUrl = new Uri(serviceUrlString);
            var client = new WebClient();
            Assert.DoesNotThrow(() =>
            {
                client.DownloadString(serviceUrl + "api/products");
            });
        }
    }
}
