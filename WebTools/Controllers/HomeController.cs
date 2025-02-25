﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Moment()
        {
            ViewBag.Message = "Break Moment with Music";

            return View();
        }

        public ActionResult MusicService() 
        {
            ViewBag.Message = "Search your Music";

            return View();
        }
    }
}