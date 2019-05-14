using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace ScrapingConsoleDemo
{
    public class AmazonItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //using (var client = new HttpClient())
            //{
            //    string html = client.GetStringAsync("http://lakewoodprogramming.com").Result;
            //    var parser = new HtmlParser();
            //    IHtmlDocument document = parser.ParseDocument(html);
            //    var element = document.QuerySelectorAll(".align-center").First();
            //    var h2 = element.QuerySelectorAll("h2")[1];
            //    Console.WriteLine(h2.TextContent);
            //}
            Console.WriteLine("Enter query");
            string query = Console.ReadLine();
            var html = GetAmazonHtml(query);
            var items = GetItems(html);
            foreach (var item in items)
            {
                Console.WriteLine(item.Price);
            }

            Console.ReadKey(true);
        }

        static string GetAmazonHtml(string query)
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "amazon is trash");
                var url = $"https://www.amazon.com/s?k=macbook";
                var html = client.GetStringAsync(url).Result;
                return html;
            }
        }

        static List<AmazonItem> GetItems(string html)
        {
            var parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            var itemDivs = document.QuerySelectorAll(".s-result-item");
            List<AmazonItem> items = new List<AmazonItem>();
            foreach (var div in itemDivs)
            {
                AmazonItem item = new AmazonItem();
                var href = div.QuerySelectorAll("a.a-text-normal").First();
                item.Title = href.TextContent.Trim();
                item.Url = href.Attributes["href"].Value;

                var image = div.QuerySelector("img.s-image");
                item.ImageUrl = image.Attributes["src"].Value;

                var priceSpan = div.QuerySelector("span.a-price-whole");
                if (priceSpan != null)
                {
                    item.Price = decimal.Parse(priceSpan.TextContent.Trim());
                }

                items.Add(item);
            }

            return items;
        }
    }
}
