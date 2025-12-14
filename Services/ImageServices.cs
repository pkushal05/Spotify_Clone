using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Clone.Services
{
    /// <summary>
    /// Represents services related to image processing and retrieval.
    /// </summary>
    public class ImageServices
    {
        // HttpClient instance for making HTTP requests.
        HttpClient Client = new HttpClient();

        /// <summary>
        /// Function to get an image from a given URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Image> GetImageFromUrl(string url)
        {
            try
            {
                // Fetch the image data as a byte array.
                byte[] bytes = await Client.GetByteArrayAsync(url);

                // Using "using" to ensure MemoryStream is disposed of properly.
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    // Create an Image from the MemoryStream and return it.
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error fetching image from URL: {ex.Message}");
                return null;
            }
        }
    }
}
