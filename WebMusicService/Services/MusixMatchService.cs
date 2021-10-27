using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebMusicService.Models;

namespace WebMusicService.Services
{
    internal class MusixMatchAPI : ITrackService, ILyricsService, IApi
    {
        public string BaseUrl
        {
            get
            {
                return "http://api.musixmatch.com/ws/1.1";
            }
        }

        public Lyrics GetLyrics(Track track, string apiKey)
        {
            string action = string.Format("/track.lyrics.get?track_id={0}&apikey={1}", track.ID, apiKey);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + action);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            JObject lyricsJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            string lyricsBody = lyricsJson["message"]["body"]["lyrics"]["lyrics_body"].ToString();

            Lyrics lyrics = new Lyrics() { Body = lyricsBody };

            return lyrics;
        }

        public List<Track> GetTracks(string searchQuery, int limit, string apiKey)
        {
            string action = string.Format("/track.search?q_track={0}&page_size={1}&page=1&s_track_rating=desc&apikey={2}", searchQuery, limit, apiKey);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + action);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            List<Track> trackList = new List<Track>();

            JArray tracksJson = (JArray)JObject.Parse(response.Content.ReadAsStringAsync().Result)["message"]["body"]["track_list"];

            foreach (var trackJson in tracksJson)
            {
                trackList.Add(new Track() { ID = trackJson["track"]["track_id"].ToString(), ArtistName = trackJson["track"]["artist_name"].ToString(), TrackName = trackJson["track"]["track_name"].ToString() });
            }

            return trackList;
        }
    }
}
