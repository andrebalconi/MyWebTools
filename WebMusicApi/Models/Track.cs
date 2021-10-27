using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMusicApi.Models
{
    public class Track
    {
        public string ID { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
    }
}