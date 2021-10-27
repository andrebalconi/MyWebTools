using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMusicService.Models;

namespace WebMusicService.Services
{
    public interface ITrackService
    {
        List<Track> GetTracks(string searchQuery, int limit, string apiKey);
    }
}
