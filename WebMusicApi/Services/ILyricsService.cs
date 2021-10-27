using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusicApi.Models;

namespace WebMusicApi.Services
{
    public interface ILyricsService
    {
        Lyrics GetLyrics(Track track, string apiKey);
    }
}