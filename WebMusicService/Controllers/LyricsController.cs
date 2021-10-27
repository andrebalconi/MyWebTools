using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMusicService.Services;
using WebMusicService.Models;
using WebMusicService.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMusicService.Extensions;

namespace WebMusicService.Controllers
{
    public class LyricsController : Controller
    {
        public IActionResult Index(string id)
        {
            List<Track> tracks = HttpContext.Session.Get<List<Track>>("tracks");
            Track selectedTrack = tracks.Where(t => t.ID == id).First();
            HttpContext.Session.Set<Track>("track", selectedTrack);

            MusixMatchAPI api = new MusixMatchAPI();

            Lyrics result = HttpContext.Session.Get<Lyrics>("lyrics");
            if (result == null)
            {
                result = api.GetLyrics(new Models.Track() { ID = id }, ApiManager.GetKey(Constants.Services.MusixMatch));
                HttpContext.Session.Set<Lyrics>("lyrics", result);
            }
            
            string token = ApiManager.GetKey(Constants.Services.Spotify);
            bool hasToken = !string.IsNullOrEmpty(token);

            List<Playlist> playlists = null;
            List<SelectListItem> playlistItems = new List<SelectListItem>();
            if (hasToken)
            {
                SpotifyService spotifyService = new SpotifyService();
                playlists = spotifyService.GetPlaylists(token);
                foreach (var pl in playlists)
                {
                    playlistItems.Add(new SelectListItem()
                    {
                        Value = pl.ID,
                        Text = pl.Name
                    });
                }
            }

            LyricsViewModel viewModel = new LyricsViewModel()
            {
                Lyrics = result,
                Playlists = playlistItems,
                HasToken = hasToken,
                TrackAdded = TempData["addedTrack"] != null
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Playlist(string playlistId)
        {
            string userId = HttpContext.Session.Get<string>("userId");

            SpotifyService service = new SpotifyService();

            Track selectedTrack = HttpContext.Session.Get<Track>("track");

            string query = selectedTrack.TrackName + " " + selectedTrack.ArtistName;

            string trackId = service.GetTracks(query, 1, ApiManager.GetKey(Constants.Services.Spotify))[0].ID;

            bool success = service.AddTrackToPlaylist(userId, playlistId, trackId, ApiManager.GetKey(Constants.Services.Spotify));

            if (success)
                TempData["addedTrack"] = true;

            return RedirectToAction("Index", new { id = selectedTrack.ID });
        }
    }


}