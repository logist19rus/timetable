using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using raspWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace raspWeb.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private AppDbCont db;
        public HomeController(AppDbCont context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("hz"))
            {
                LolModel asd = new LolModel();
                asd.asd = HttpContext.Session.GetString("hz");
                asd.dsa = 123;
                return View(asd);
            }

            return View();
        }

        public IActionResult clear()
        {
            db.Paras.RemoveRange(db.Paras.ToList());
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult TableOfRasp(string group)
        {
            List<string> pary = new List<string>();
            pary.Add("para1");
            pary.Add("para2");
            pary.Add("para3");
            return PartialView(pary);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
