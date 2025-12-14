using Spotify_Clone.DTOs;
using Spotify_Clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.Converters
{
    public static class TrackMapper
    {
        /// <summary>
        /// Converts TrackDTO to Track model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Track FromDTO(TrackDTO dto)
        {
            return new Track
            {
                Id = dto.id,
                Name = dto.name,
                Artist = dto.artists != null && dto.artists.Count > 0 ? dto.artists[0].name : "Unknown Artist",
                CoverImage = dto.album?.images != null && dto.album.images.Count > 0 ? dto.album.images[0].url : string.Empty,
                Album = dto.album?.name ?? "Unknown Album",
                Duration = dto.duration_ms

            };
        }

        /// <summary>
        /// Converts a list of TrackDTO to a list of Track models
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        public static List<Track> FromDTOList(List<TrackDTO> dtoList)
        {
            List<Track> tracks = new List<Track>();
            foreach (TrackDTO dto in dtoList)
            {
                tracks.Add(FromDTO(dto));
            }
            return tracks;
        }
    }
}
