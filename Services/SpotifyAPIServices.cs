using Spotify_Clone.Config;
using Spotify_Clone.Converters;
using Spotify_Clone.DTOs;
using Spotify_Clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spotify_Clone.Services
{
    /// <summary>
    /// Represents services related to Spotify API interactions.
    /// </summary>
    public class SpotifyAPIServices
    {
        // HttpClient instance for making HTTP requests.
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Authorization header is set using the access token from AppConfig.
        /// </summary>
        public SpotifyAPIServices()
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppConfig.AccessToken}");
        }

        /// <summary>
        /// Gets a list of albums based on the search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<Album>> GetAlbum(string query)
        {
            // Construct the URL for album search.
            string AlbumsUrl = $"{AppConfig.BaseUrl}search?q={query}&type=album&limit=50";

            try
            {
                // Make the GET request to the Spotify API.
                HttpResponseMessage result = await client.GetAsync(AlbumsUrl);

                // Check if the request was successful.
                if (result.IsSuccessStatusCode)
                {
                    string jsonResponse = await result.Content.ReadAsStringAsync();
                    AlbumSearchResultDTO dto = JsonSerializer.Deserialize<AlbumSearchResultDTO>(jsonResponse);

                    return AlbumMapper.FromDTOList(dto?.albums?.items);
                }
                else
                {
                    MessageBox.Show($"Spotify API request failed with status code: {result.RequestMessage}");
                    return new List<Album>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching albums from Spotify API: {ex.Message}");
                return new List<Album>();
            }
        }

        /// <summary>
        /// Gets a list of tracks based on the search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<Track>> GetTracks(string query)
        {
            // Construct the URL for track search.
            string TracksUrl = $"{AppConfig.BaseUrl}search?q={query}&type=track&limit=20";
            try
            {
                // Make the GET request to the Spotify API.
                HttpResponseMessage result = await client.GetAsync(TracksUrl);

                // Check if the request was successful.
                if (result.IsSuccessStatusCode)
                {
                    string jsonResponse = await result.Content.ReadAsStringAsync();
                    TrackSearchResultDTO dto = JsonSerializer.Deserialize<TrackSearchResultDTO>(jsonResponse);
                    return TrackMapper.FromDTOList(dto?.tracks?.items);
                }
                else
                {
                    MessageBox.Show($"Spotify API request failed with status code: {result.RequestMessage}");
                    return new List<Track>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching tracks from Spotify API: {ex.Message}");
                return new List<Track>();
            }
        }

        /// <summary>
        /// Gets a list of artists based on the search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<Artist>> GetArtists(string query)
        {
            // Construct the URL for artist search.
            string ArtistsUrl = $"{AppConfig.BaseUrl}search?q={query}&type=artist&limit=20";
            try
            {
                // Make the GET request to the Spotify API.
                HttpResponseMessage result = await client.GetAsync(ArtistsUrl);

                // Check if the request was successful.
                if (result.IsSuccessStatusCode)
                {
                    string jsonResponse = await result.Content.ReadAsStringAsync();
                    ArtistSearchResultDTO dto = JsonSerializer.Deserialize<ArtistSearchResultDTO>(jsonResponse);
                    return ArtistMapper.FromDTOList(dto?.artists?.items);
                }
                else
                {
                    MessageBox.Show($"Spotify API request failed with status code: {result.RequestMessage}");
                    return new List<Artist>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching artists from Spotify API: {ex.Message}");
                return new List<Artist>();
            }
        }

        /// <summary>
        /// Gets detailed information about a specific album by its ID.
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task<Album> GetAlbumDetails(string albumId)
        {
            // Construct the URL for album details.
            string AlbumDetailsUrl = $"{AppConfig.BaseUrl}albums/{albumId}";
            try
            {
                // Make the GET request to the Spotify API.
                HttpResponseMessage result = await client.GetAsync(AlbumDetailsUrl);

                // Check if the request was successful.
                if (result.IsSuccessStatusCode)
                {
                    string jsonResponse = await result.Content.ReadAsStringAsync();
                    AlbumDTO dto = JsonSerializer.Deserialize<AlbumDTO>(jsonResponse);
                    return AlbumMapper.FromDTO(dto);
                }
                else
                {
                    MessageBox.Show($"Spotify API request failed with status code: {result.RequestMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching album details from Spotify API: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets detailed information about a specific artist by its ID.
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        public async Task<Artist> GetArtistDetails(string artistId)
        {
            // Construct the URLs for artist details and top tracks.
            string ArtistDetailsUrl = $"{AppConfig.BaseUrl}artists/{artistId}";
            string ArtistsTracksUrl = $"{AppConfig.BaseUrl}artists/{artistId}/top-tracks";
            try
            {
                // Make the GET requests to the Spotify API.
                HttpResponseMessage result = await client.GetAsync(ArtistDetailsUrl);
                HttpResponseMessage trackResult = await client.GetAsync(ArtistsTracksUrl);

                // Check if the requests were successful.
                if (result.IsSuccessStatusCode && trackResult.IsSuccessStatusCode)
                {
                    string jsonResponse = await result.Content.ReadAsStringAsync();
                    string trackJsonResponse = await trackResult.Content.ReadAsStringAsync();
                    ArtistDTO dto = JsonSerializer.Deserialize<ArtistDTO>(jsonResponse);
                    ArtistTopTrackDTO trackDtos = JsonSerializer.Deserialize<ArtistTopTrackDTO>(trackJsonResponse);
                    Artist artist = ArtistMapper.FromDTO(dto);
                    artist.TopTracks = TrackMapper.FromDTOList(trackDtos?.tracks);
                    return artist;
                }
                else
                {
                    MessageBox.Show($"Spotify API request failed with status code: {result.RequestMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching album details from Spotify API: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets detailed information about a specific track by its ID.
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public async Task<Track> GetTrackDetails(string trackId)
        {
            // Construct the URL for track details.
            string TrackDetailsUrl = $"{AppConfig.BaseUrl}tracks/{trackId}";
            try
            {
                // Make the GET request to the Spotify API.
                HttpResponseMessage result = await client.GetAsync(TrackDetailsUrl);

                // Check if the request was successful.
                if (result.IsSuccessStatusCode)
                {
                    string jsonResponse = await result.Content.ReadAsStringAsync();
                    TrackDTO dto = JsonSerializer.Deserialize<TrackDTO>(jsonResponse);
                    return TrackMapper.FromDTO(dto);
                }
                else
                {
                    MessageBox.Show($"Spotify API request failed with status code: {result.RequestMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching track details from Spotify API: {ex.Message}");
                return null;
            }
        }
    }
}
