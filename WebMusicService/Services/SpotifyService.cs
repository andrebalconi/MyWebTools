using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebMusicService.Models;

namespace WebMusicService.Services
{
    public class SpotifyService : ITrackService, IApi
    {
        private const string accountUrl = "https://accounts.spotify.com";

        public string BaseUrl
        {
            get
            {
                return "https://api.spotify.com";
            }
        }

        public void GenerateToken(string code)
        {
            string action = "/api/token";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, accountUrl + action);

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            parameters.Add(new KeyValuePair<string, string>("code", code));
            parameters.Add(new KeyValuePair<string, string>("redirect_uri", "http://localhost:52170/Auth/Index/"));
            parameters.Add(new KeyValuePair<string, string>("client_id", "9e83ce9f36ae4d97bbd53a5521791c52"));
            parameters.Add(new KeyValuePair<string, string>("client_secret", "f5288f677f834eeebfe2e62f44d3b005"));

            request.Content = new FormUrlEncodedContent(parameters);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            JObject tokenJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            string token = tokenJson["access_token"].ToString();

            ApiManager.AddKey(Constants.Services.Spotify, token);
        }

        public List<Playlist> GetPlaylists(string token)
        {
            string action = "/v1/me/playlists";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, this.BaseUrl + action);

            request.Headers.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            JArray playlistsJson = (JArray)JObject.Parse(response.Content.ReadAsStringAsync().Result)["items"];

            List<Playlist> playlists = new List<Playlist>();

            foreach (var playlistJson in playlistsJson)
            {
                playlists.Add(new Playlist() { ID = playlistJson["id"].ToString(), Name = playlistJson["name"].ToString() });
            }

            return playlists;
        }

        public string GetUserId(string token)
        {
            string action = "/v1/me";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, this.BaseUrl + action);

            request.Headers.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            JObject userJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            string userId = userJson["id"].ToString();

            return userId;
        }

        public bool AddTrackToPlaylist(string userId, string playlistId, string trackId, string token)
        {
            string action = string.Format("/v1/users/{0}/playlists/{1}/tracks?uris=spotify:track:{2}", userId, playlistId, trackId);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.BaseUrl + action);

            request.Headers.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            return response.IsSuccessStatusCode;
        }

        public List<Track> GetTracks(string searchQuery, int limit, string token)
        {
            string action = string.Format("/v1/search?q={0}&type=track&limit={1}", searchQuery, limit);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, this.BaseUrl + action);

            request.Headers.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = HttpInstance.GetHttpClientInstance().SendAsync(request).Result;

            JArray tracksJson = (JArray)JObject.Parse(response.Content.ReadAsStringAsync().Result)["tracks"]["items"];

            List<Track> tracks = new List<Track>();

            foreach (var trackJson in tracksJson)
            {
                tracks.Add(new Track() { ID = trackJson["id"].ToString(), ArtistName = trackJson["name"].ToString() });
            }

            return tracks;
        }
    }
}
