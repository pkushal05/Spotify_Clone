using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.DTOs
{
    /// <summary>
    /// Represents the result of an artist search operation.
    /// </summary>
    public class ArtistSearchResultDTO
    {
        public ArtistContainerDTO artists { get; set; }
    }

    /// <summary>
    /// Represents a container for a collection of artist data transfer objects.
    /// </summary>
    public class ArtistContainerDTO
    {
        public List<ArtistDTO> items { get; set; }
    }

    /// <summary>
    /// Represents an artist, including their basic information, popularity, followers, and associated images.
    /// </summary>
    public class ArtistDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<ImageDTO> images { get; set; }
        public int popularity { get; set; }
        public FollowerDTO followers { get; set; }
    }
}
