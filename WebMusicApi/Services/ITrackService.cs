using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusicApi.Models;

namespace WebMusicApi.Services
{
    public interface ITrackService
    {
        List<Track> GetTracks(string searchQuery, int limit, string apiKey);
    }
}