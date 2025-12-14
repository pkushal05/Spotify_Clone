using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.DTOs
{
    /// <summary>
    /// Represents the result of a track search operation, containing the tracks that match the search criteria.
    /// </summary>
    public class TrackSearchResultDTO
    {
        public TrackContainerDTO tracks { get; set; }
    }

    /// <summary>
    /// Represents a container for a collection of tracks.
    /// </summary>
    public class TrackContainerDTO
    {
        public List<TrackDTO> items { get; set; }
    }

    /// <summary>
    /// Represents a data transfer object (DTO) for a music track, including its metadata such as ID, name, artists,
    /// album, and duration.
    /// </summary>
    public class TrackDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<ArtistDTO> artists { get; set; }
        public AlbumDTO album { get; set; }
        public int duration_ms { get; set; }
    }

    /// <summary>
    /// Represents a data transfer object containing the top tracks of an artist.
    /// </summary>
    public class ArtistTopTrackDTO
    {
        public List<TrackDTO> tracks { get; set; }
    }
}
