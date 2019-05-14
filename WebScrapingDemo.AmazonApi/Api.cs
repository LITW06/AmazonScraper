using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace WebScrapingDemo.AmazonApi
{
    public static class Api
    {
        public static List<AmazonItem> ScrapeAmazon(string query)
        {
            var html = GetAmazonHtml(query);
            return GetItems(html);
        }

        private static string GetAmazonHtml(string query)
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "amazon is trash");
                var url = $"https://www.amazon.com/s?k={query}";
                var html = client.GetStringAsync(url).Result;
                return html;
            }
        }

        private static List<AmazonItem> GetItems(string html)
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