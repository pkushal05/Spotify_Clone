using Spotify_Clone.DTOs;
using Spotify_Clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.Converters
{
    public static class ArtistMapper
    {
        /// <summary>
        /// Converts ArtistDTO to Artist model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Artist FromDTO(ArtistDTO dto)
        {
            return new Artist
            {
                Id = dto.id,
                Name = dto.name,
                Popularity = dto.popularity,
                Followers = dto.followers.total,
                CoverImage = dto.images.Count > 0 ? dto.images[0].url : string.Empty,
            };
        }

        /// <summary>
        /// Converts a list of ArtistDTO to a list of Artist models
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        public static List<Artist> FromDTOList(List<ArtistDTO> dtoList)
        {
            List<Artist> artists = new List<Artist>();
            foreach (ArtistDTO dto in dtoList)
            {
                artists.Add(FromDTO(dto));
            }
            return artists;
        }
    }
}
