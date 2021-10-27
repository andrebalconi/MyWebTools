using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMusicService.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebMusicService.Models;
using WebMusicService.Extensions;

namespace WebMusicService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
