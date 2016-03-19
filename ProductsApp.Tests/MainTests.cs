using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace ProductsApp.Tests
{
    [TestFixture]
    public class MainTests
    {
        private string _serviceUrlString;

        [SetUp]
        public void BeforeAll()
        {
            //get the base service url from the App.config
            _serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];
        }

        [Test]
        public void GetAllProducts_returns_200_code()
        {
            //create an instance of Uri class to validate the service url
            var serviceUrl = new Uri(_serviceUrlString);
            var client = new WebClient();
            Assert.DoesNotThrow(() =>
            {
                client.DownloadString(serviceUrl + "api/products");
            });
        }

        [Test]
        public async Task GetAllProducts_returns_products()
        {
            var serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];
            var serviceUrl = new Uri(serviceUrlString);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var response = await client.GetAsync(serviceUrl + "api/products");

            // gets the content of http responce and converts it to a string
            var result = response.Content.ReadAsStringAsync().Result;

            var xmlDocument = XDocument.Parse(result);
            if (xmlDocument.Root != null)
            {
                var nodes = xmlDocument.Root.Descendants().Select(x => x.Value).ToString();
                Assert.AreEqual("Tomato Soup $1", nodes[0].ToString());
            }

        }


        [Test]
        public async Task GetAllProducts_returns_products2()
        {
            var serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];
            var serviceUrl = new Uri(serviceUrlString);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();


            StreamReader ResponseDataStream = new StreamReader(response.GetResponseStream());
            String res = ResponseDataStream.ReadToEnd();

            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.LoadXml(res);
        }

            var xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            xdoc.Root.Descendants().Select(x => x.Value).ToArray();

        }
    }
}
