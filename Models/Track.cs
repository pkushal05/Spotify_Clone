using Spotify_Clone.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Spotify_Clone.Models
{
    /// <summary>
    /// Represents a Track Model
    /// </summary>
    public class Track
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string CoverImage { get; set; }
        public int Duration { get; set; }

        /// <summary>
        /// Function to format duration from milliseconds to minutes:seconds
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string FormatDuration(int ms)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            return $"{ts.Minutes}:{ts.Seconds:D2}";
        }

        /// <summary>
        /// Function to save track details to a JSON file
        /// </summary>
        /// <param name="trackDetails"></param>
        public static void SaveTrackJson(Track trackDetails)
        {
            string FileName = "trackDetails.json";

            List<Track> newTrackList = new List<Track>();

            // Check if file exists
            if (File.Exists(FileName))
            {
                // Read the current content of the file
                string existingJson = File.ReadAllText(FileName);
                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    newTrackList = JsonSerializer.Deserialize<List<Track>>(existingJson);
                    try
                    {
                        // Check if the track already exists in the list
                        int indexToRemove = newTrackList.FindIndex(t => t.Id == trackDetails.Id);
                        if (indexToRemove != -1)
                        {
                            // If it exists, remove it
                            newTrackList.RemoveAt(indexToRemove);
                            string removedTackList = JsonSerializer.Serialize(newTrackList);
                            File.WriteAllText(FileName, removedTackList);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating track details: {ex.Message}");
                    }
                    // Add the new track details to the list
                    newTrackList.Add(trackDetails);

                    // Write the updated list back to the file
                    string updatedJson = JsonSerializer.Serialize(newTrackList);
                    File.WriteAllText(FileName, updatedJson);
                }
            }
            else
            {
                // If the file doesn't exist, create a new list with the track details
                newTrackList.Add(trackDetails);
                string newJson = JsonSerializer.Serialize(newTrackList);
                File.WriteAllText(FileName, newJson);
            }
        }

        /// <summary>
        /// Function to check if a track is already saved in the JSON file
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public static bool IsTrackSaved(string trackId)
        {
            string FileName = "trackDetails.json";
            if (File.Exists(FileName))
            {
                string existingJson = File.ReadAllText(FileName);
                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    List<Track> existingTracks = JsonSerializer.Deserialize<List<Track>>(existingJson);
                    return existingTracks.Any(t => t.Id == trackId);
                }
            }
            return false;
        }

        /// <summary>
        /// Function to load saved tracks from the JSON file
        /// </summary>
        /// <returns></returns>
        public static List<Track> LoadSavedTracks()
        {
            string FileName = "trackDetails.json";
            try
            {
                if (File.Exists(FileName))
                {
                    string existingJson = File.ReadAllText(FileName);
                    if (!string.IsNullOrWhiteSpace(existingJson))
                    {
                        return JsonSerializer.Deserialize<List<Track>>(existingJson);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Error loading saved tracks: {ex.Message}");
            }
            return new List<Track>();
        }
    }
}
