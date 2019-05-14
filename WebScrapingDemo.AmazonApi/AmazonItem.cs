using System;

namespace WebScrapingDemo.AmazonApi
{
    public class AmazonItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
    }
}
