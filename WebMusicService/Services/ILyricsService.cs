using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMusicService.Models;

namespace WebMusicService.Services
{
    public interface ILyricsService
    {
        Lyrics GetLyrics(Track track, string apiKey);
    }
}
