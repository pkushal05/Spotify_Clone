using Spotify_Clone.DTOs;
using Spotify_Clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.Converters
{
    public class AlbumMapper
    {
        /// <summary>
        /// Converts AlbumDTO to Album model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Album FromDTO(AlbumDTO dto)
        {
            return new Album
            {
                Id = dto.id,
                Name = dto.name,
                Artist =dto.artists.Count > 0 ? dto.artists[0].name : "Unknown Artist",
                ReleaseDate = dto.release_date,
                CoverImage =dto.images.Count > 0 ? dto.images[0].url : string.Empty,
                Tracks = dto.tracks?.items?.Select(TrackMapper.FromDTO).ToList() ?? new List<Track>()
            };
        }

        /// <summary>
        /// Converts a list of AlbumDTO to a list of Album models
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        public static List<Album> FromDTOList(List<AlbumDTO> dtoList)
        {
            List<Album> albums = new List<Album>();
            foreach (AlbumDTO dto in dtoList)
            {
                albums.Add(FromDTO(dto));
            }
            return albums;
        }
    }
}
