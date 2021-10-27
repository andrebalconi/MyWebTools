using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMusicApi.Models;
using WebMusicApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebMusicApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return (IActionResult)View();
        }
        public IActionResult Search(string searchQuery)
        {
            MusixMatchAPI api = new MusixMatchAPI();
            var result = api.GetTracks(searchQuery, 5, ApiManager.GetKey(Constants.Services.MusixMatch));
            HttpContext.Session.Set<List<Track>>("tracks", result);
            return View("Index", result);
        }

    }
}