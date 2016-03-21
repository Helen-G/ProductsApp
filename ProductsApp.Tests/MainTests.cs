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

        [OneTimeSetUp]
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
            //get the service url
            var serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];
            var serviceUri = new Uri(serviceUrlString);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var response = await client.GetAsync(serviceUri + "api/products");

            // gets the content of http responce and converts it to a string
            var result = response.Content.ReadAsStringAsync().Result;

            /*var xmlDocument = XDocument.Parse(result);
            if (xmlDocument.Root != null)
            {
                //var nodes = xmlDocument.Root.Descendants().Select(x => x.Value).ToString();
                var nodes = xmlDocument.Root.Elements().Select(x => x.Name == "Product");

                var nodes2 = xmlDocument.Root.Descendants();
                var elements = nodes2.Elements();
                elements.Get
            }*/

            //parse the response to xml
            var document = new XmlDocument();
            document.LoadXml(result);

            //check that the service returns 3 products
            var products = document.GetElementsByTagName("Product");
            Assert.AreEqual(3, products.Count);
        }


        [Test]
        public async Task GetProduct_returns_product()
        {
            //get the service url
            var serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];
            var serviceUri = new Uri(serviceUrlString);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var response = await client.GetAsync(serviceUri + "api/products/1");

            // gets the content of http responce and converts it to a string
            var result = response.Content.ReadAsStringAsync().Result;

            //parse the response to xml
            var document = new XmlDocument();
            document.LoadXml(result);

            //check the data of the first product
            var productAttributes = document.GetElementsByTagName("Product")[0].ChildNodes;
            var productCategory = productAttributes[0].InnerText;
            var productId = productAttributes[1].InnerText;
            var productName = productAttributes[2].InnerText;
            var productPrice = productAttributes[3].InnerText;

            Assert.AreEqual("Groceries", productCategory);
            Assert.AreEqual("1", productId);
            Assert.AreEqual("Tomato Soup", productName);
            Assert.AreEqual("1", productPrice);
        }

        [Test]
        public async Task Test_to_rewrite()
        {
            var serviceUrlString = ConfigurationManager.AppSettings["ServiceUrl"];
            var serviceUrl = new Uri(serviceUrlString + "api/products");
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            //create a request
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl + );

            WebRequest request = WebRequest.Create(serviceUrl);

            //get a response
            //HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            WebResponse response = request.GetResponse();
            using (var sr = new StreamReader(stream: response.GetResponseStream()))
            {
                XDocument xmlDoc = new XDocument();

                xmlDoc = XDocument.Parse(sr.ReadToEnd());
                var status = xmlDoc.Root.Element("Product").Value;
                //Console.WriteLine("Response status: {0}", status);
                //if (status == "OK")
                {
                    // if the status is OK it's normally safe to assume the required elements
                    // are there. However, if you want to be safe you can always check the element
                    // exists before retrieving the value
                    Console.WriteLine(xmlDoc.Root.Element("CountryCode").Value);
                    Console.WriteLine(xmlDoc.Root.Element("CountryName").Value);

                }
            }

            //create a response data stream
            //StreamReader ResponseDataStream = new StreamReader(response.GetResponseStream());
            //String res = ResponseDataStream.ReadToEnd();

            //parse a HttpWebResponse to xml
            //var xmldDoc = new XmlDocument();
            //xmldDoc.Load(response.GetResponseStream());
            //var product1 =  xmldDoc.GetElementsByTagName("Product")[0].Attributes;


            //var xdoc = XDocument.Parse(res);
            //xmldDoc.LoadXml(res);


        }
    }
}
