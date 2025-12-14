using Spotify_Clone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Spotify_Clone
{
    /// <summary>
    /// Form to display the user's liked tracks.
    /// </summary>
    public partial class LikedTracksForm : Form
    {
        public LikedTracksForm()
        {
            InitializeComponent();

            // Initialize the ListView and load tracks when the form is created
            InitializeListView();
            LoadTracksIntoListView();

        }

        /// <summary>
        /// Adds columns to the ListView for displaying track information.
        /// </summary>
        public void InitializeListView()
        {
            ListView.Columns.Add("Track", 270);
            ListView.Columns.Add("Artist", 202);
            ListView.Columns.Add("Duration", 80);
        }

        /// <summary>
        /// Loads saved tracks and populates the ListView with their details.
        /// </summary>
        public void LoadTracksIntoListView()
        {
            ListView.Items.Clear();
            
            List<Track> savedTracks = Track.LoadSavedTracks();

            foreach (Track track in savedTracks)
            {
                string formattedDuration = Track.FormatDuration(track.Duration);
                AddTrackToListView(track.Name, track.Artist, formattedDuration);
            }
        }

        /// <summary>
        /// Function to add a track's details to the ListView.
        /// </summary>
        /// <param name="trackName"></param>
        /// <param name="artistName"></param>
        /// <param name="duration"></param>
        public void AddTrackToListView(string trackName, string artistName, string duration)
        {
            ListViewItem item = new ListViewItem(trackName);
            item.SubItems.Add(artistName);
            item.SubItems.Add(duration);
            ListView.Items.Add(item);
        }

        /// <summary>
        /// Removes the selected track from the liked tracks list and updates the JSON file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLikedBtn_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (ListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a track to remove.");
                return;
            }

            // Read the existing JSON file
            string json = File.ReadAllText("trackDetails.json");
            List<Track> trackList = JsonSerializer.Deserialize<List<Track>>(json);

            // Remove the track from the list based on the selected index
            int indexToRemove = ListView.SelectedIndices[0];
            trackList.RemoveAt(indexToRemove);
            ListView.SelectedItems[0].Remove();

            // Write the updated list back to the JSON file
            string updatedJson = JsonSerializer.Serialize(trackList);
            File.WriteAllText("trackDetails.json", updatedJson);
        }
    }
}
