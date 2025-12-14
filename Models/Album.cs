using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.Models
{
    /// <summary>
    /// Respresents a Album Model
    /// </summary>
    public class Album
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string ReleaseDate { get; set; }
        public string CoverImage { get; set; }
        public List<Track> Tracks { get; set; } = new List<Track>();
    }
}
