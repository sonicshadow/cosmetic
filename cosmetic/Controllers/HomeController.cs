using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;

namespace Cosmetic.Controllers
{

    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var did = Bll.SystemSettings.Display;
            var d = db.Displays.Where(s => did.Contains(s.ID)).ToList()
                .Select(s=>new DisplayViewModel(s)).ToList();
            var n = db.Notices.OrderByDescending(s => s.CreateTime).Take(5).ToList();
            var model = new Home()
            {
                Display = d,
                Notice = n,
            };
            return View(model);
        }

        public ActionResult About()
        {
            var mission = new MissionCompile()
            {
                Value = Bll.SystemSettings.Contact
            };
            return View(mission);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}