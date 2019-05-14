using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebScrapingDemo.AmazonApi;
using WebScrapingDemo.Web.Models;

namespace WebScrapingDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string query)
        {
            List<AmazonItem> items = Api.ScrapeAmazon(query);
            return View(items);
        }
    }
}
