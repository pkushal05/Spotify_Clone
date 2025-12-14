using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.Models
{
    /// <summary>
    /// Representes an Artist Model
    /// </summary>
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Popularity { get; set; }
        public int Followers { get; set; }
        public string CoverImage { get; set; }
        public List<Track> TopTracks { get; set; } = new List<Track>();
    }
}
