using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMusicService.Services;
using WebMusicService.Extensions;
using WebMusicService.Models;

namespace WebMusicService.Controllers
{
    public class AuthController : Controller
    {        
        public IActionResult Index()
        {
            string code = HttpContext.Request.Query["code"];
            GenerateToken(code);
            Track selectedTrack = HttpContext.Session.Get<Track>("track");

            SpotifyService spotifyService = new SpotifyService();

            string userId = HttpContext.Session.Get<string>("userId");
            if (string.IsNullOrEmpty(userId))
                userId = spotifyService.GetUserId(ApiManager.GetKey(Constants.Services.Spotify));

            HttpContext.Session.Set<string>("userId", userId);

            return RedirectToAction("Index", "Lyrics", new { id = selectedTrack.ID });
        }

        private void GenerateToken(string code)
        {
            SpotifyService spotifyService = new SpotifyService();
            spotifyService.GenerateToken(code);
        }
    }
}