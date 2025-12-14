using Spotify_Clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.DTOs
{
    /// <summary>
    /// Represents the result of an album search operation.
    /// </summary>
    public class AlbumSearchResultDTO
    {
        public AlbumContainerDTO albums { get; set; }
    }

    /// <summary>
    /// Represents a container for a collection of albums.
    /// </summary>
    public class AlbumContainerDTO
    {
        public List<AlbumDTO> items { get; set; }
    }

    /// <summary>
    /// Represents an album, including its metadata, associated artists, release date, images, and tracks.
    /// </summary>
    public class AlbumDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<ArtistDTO> artists { get; set; }
        public string release_date { get; set; }
        public List<ImageDTO> images { get; set; }
        public TrackContainerDTO tracks { get; set; }
    }
}
