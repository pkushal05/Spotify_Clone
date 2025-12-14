using Spotify_Clone.Config;
using Spotify_Clone.Models;
using Spotify_Clone.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Spotify_Clone
{
    public partial class Form1 : Form
    {
        // Fields
        private Button activeButton;
        private Track trackDetails;

        public Form1()
        {
            InitializeComponent();

            // Initial setup
            SpawnTestCards();
            SetActiveButon(AlbumsBtn);
        }

        /// <summary>
        /// Spawn mock data so we can see the layout
        /// </summary>
        private void SpawnTestCards()
        {
            // Clear any existing cards
            flowLayoutPanel1.Controls.Clear();

            // Spawn 5 test cards
            for (int i = 0; i < 5; i++)
            {
                AlbumCard albumCard = new AlbumCard();
                flowLayoutPanel1.Controls.Add(albumCard);
            }
        }

        /// <summary>
        /// Gets the active button and sets its color
        /// </summary>
        /// <param name="ActiveButton"></param>
        private void SetActiveButon(Button ActiveButton)
        {
            Color green = Color.FromArgb(30, 215, 96);
            Color black = Color.Black;

            // Reset all buttons
            foreach (Button btn in new[] { AlbumsBtn, ArtistsBtn, TracksBtn })
            {
                btn.BackColor = black;
                btn.ForeColor = green;
            }

            // Set active button color
            ActiveButton.BackColor = green;
            ActiveButton.ForeColor = black;

            // Update active button reference
            activeButton = ActiveButton;
        }

        // Button Click Handlers
        private void AlbumsBtn_Click(object sender, EventArgs e)
        {

            SetActiveButon((Button)sender);
            LikeSongBtn.Visible = false;
        }

        // Button Click Handlers
        private void ArtistsBtn_Click(object sender, EventArgs e)
        {
            SetActiveButon((Button)sender);
            LikeSongBtn.Visible = false;
        }

        // Button Click Handlers
        private void TracksBtn_Click(object sender, EventArgs e)
        {
            SetActiveButon((Button)sender);
            LikeSongBtn.Visible = true;
        }

        /// <summary>
        /// Validates the query and performs search based on active button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string query = QueryTextBox.Text;

            // Validate query
            if (string.IsNullOrWhiteSpace(query) || int.TryParse(query, out _))
            {
                MessageBox.Show("Please enter a valid search query.",
                                "Warning!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                QueryTextBox.Clear();
                return;
            }

            // Perform search based on active button
            if (activeButton == AlbumsBtn)
            {
                SearchAlbums(query);
            }
            else if (activeButton == ArtistsBtn)
            {
                SearchArtists(query);
            }
            else if (activeButton == TracksBtn)
            {
                SearchTracks(query);
            }
        }

        /// <summary>
        /// Updates the side panel with provided data
        /// </summary>
        /// <param name="coverImage"></param>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <param name="subTitle2"></param>
        private void UpdateSidePanel(Image coverImage, string title, string subTitle, string subTitle2)
        {
            CoverImage.Image = coverImage;
            TitleLabel.Text = title;
            SubTitleLabel1.Text = subTitle;
            SubTitleLabel2.Text = subTitle2;
        }

        /// <summary>
        /// Makes API call to search for albums and displays them
        /// </summary>
        /// <param name="query"></param>
        private async void SearchAlbums(string query)
        {
            SpotifyAPIServices SpotifyClient = new SpotifyAPIServices();

            List<Album> albums = await SpotifyClient.GetAlbum(query);
           
            // Clear existing cards
            flowLayoutPanel1.Controls.Clear();
            // Display fetched albums
            try {
                foreach (Album album in albums)
                {
                    // Render album cards based on fetched data
                    AlbumCard albumCard = new AlbumCard();

                    // Fetch album art
                    ImageServices imageService = new ImageServices();
                    Image albumArt = await imageService.GetImageFromUrl(album.CoverImage);

                    // Set album data on the card
                    albumCard.SetData(album.Id, album.Name, albumArt);
                    flowLayoutPanel1.Controls.Add(albumCard);

                    // Attach click event handler
                    albumCard.Click += HandleCardClick;
                }
            } 
            catch (Exception ex) 
            {
                // Handle any errors that occur during album display
                MessageBox.Show($"Error fetching albums: {ex.Message}", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error); 
                return;
            }
            
        }

        /// <summary>
        /// Makes API call to fetch details based on clicked card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandleCardClick(object sender, EventArgs e)
        {
            // Get the clicked album card
            AlbumCard albumCard = sender as AlbumCard;

            // Get its ID
            string ID = albumCard.ID;
            try
            {
                SpotifyAPIServices SpotifyClient = new SpotifyAPIServices();
                if (activeButton == AlbumsBtn)
                {
                    // Fetch album details if Albums button is active
                    Album albumDetails = await SpotifyClient.GetAlbumDetails(ID);
                    // Fetch album cover image
                    ImageServices imageService = new ImageServices();
                    Image coverImage = await imageService.GetImageFromUrl(albumDetails.CoverImage);
                    // Update side panel with album details
                    UpdateSidePanel(coverImage, albumDetails.Name, albumDetails.Artist, albumDetails.ReleaseDate);
                    DisplayTracks(albumDetails.Tracks);
                }
                else if (activeButton == ArtistsBtn)
                {
                    // Fetch artist deatils if Artists button is active
                    Artist artistDetails = await SpotifyClient.GetArtistDetails(ID);
                    // Fetch artist cover image
                    ImageServices imageService = new ImageServices();
                    Image coverImage = await imageService.GetImageFromUrl(artistDetails.CoverImage);
                    // Update side panel with artist details
                    UpdateSidePanel(coverImage, artistDetails.Name, $"Followers: {artistDetails.Followers}", $"Popularity: {artistDetails.Popularity}");
                    DisplayTracks(artistDetails.TopTracks);
            }
                else if (activeButton == TracksBtn)
                {
                    // Fetch track details if Tracks button is active
                    Track trackDetails = await SpotifyClient.GetTrackDetails(ID);
                    // Store track details for like/unlike functionality
                    this.trackDetails = trackDetails;
                    // Fetch track cover image
                    ImageServices imageService = new ImageServices();
                    Image coverImage = await imageService.GetImageFromUrl(trackDetails.CoverImage);
                    // Update side panel with track details
                    UpdateSidePanel(coverImage, trackDetails.Name, trackDetails.Artist, Track.FormatDuration(trackDetails.Duration));
                    List<Track> trackList = new List<Track> { trackDetails };
                    DisplayTracks(trackList);

                    // Update Like/Unlike button text based on saved status
                    LikeSongBtn.Text = Track.IsTrackSaved(trackDetails.Id) ? "Unlike Song" : "Like Song";
            }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during detail fetching
                MessageBox.Show($"Error fetching album details: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets artists from API and displays them
        /// </summary>
        /// <param name="query"></param>
        private async void SearchArtists(string query) 
        {
            SpotifyAPIServices SpotifyClient = new SpotifyAPIServices();

            // Make API call to fetch artists
            List<Artist> artists = await SpotifyClient.GetArtists(query);

            // Clear existing cards
            flowLayoutPanel1.Controls.Clear();
            // Display fetched albums
            try
            {
                ImageServices imageService = new ImageServices();
                foreach (Artist artist in artists)
                {
                    // Render artist cards based on fetched data
                    AlbumCard albumCard = new AlbumCard();

                    // Fetch artist image
                    Image albumArt = await imageService.GetImageFromUrl(artist.CoverImage);

                    // Set artist data on the card
                    albumCard.SetData(artist.Id, artist.Name, albumArt);
                    flowLayoutPanel1.Controls.Add(albumCard);

                    // Attach click event handler
                    albumCard.Click += HandleCardClick;

                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during artist display
                MessageBox.Show($"Error fetching albums: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Makes API call to search for tracks and displays them
        /// </summary>
        /// <param name="query"></param>
        private async void SearchTracks(string query)
        {
            SpotifyAPIServices SpotifyClient = new SpotifyAPIServices();

            // Make API call to fetch tracks
            List<Track> tracks = await SpotifyClient.GetTracks(query);

            // Clear existing cards
            flowLayoutPanel1.Controls.Clear();
            // Display fetched albums
            try
            {
                ImageServices imageService = new ImageServices();
                foreach (Track track in tracks)
                {
                    // Render track cards based on fetched data
                    AlbumCard albumCard = new AlbumCard();

                    // Fetch track cover image
                    Image albumArt = await imageService.GetImageFromUrl(track.CoverImage);

                    // Set track data on the card
                    albumCard.SetData(track.Id, track.Name, albumArt);
                    flowLayoutPanel1.Controls.Add(albumCard);

                    // Attach click event handler
                    albumCard.Click += HandleCardClick;

                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during track display
                MessageBox.Show($"Error fetching albums: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Add columns to the ListView on form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            listView4.Columns.Add("Track", 200);
            listView4.Columns.Add("Artist", 150);
            listView4.Columns.Add("Duration", 100);

            // Hide Like Song button initially
            LikeSongBtn.Visible = false;
        }

        /// <summary>
        /// Function to display a list of tracks in the ListView.
        /// </summary>
        /// <param name="tracks"></param>
        private void DisplayTracks(List<Track> tracks)
        {
            // Clear existing items
            listView4.Items.Clear();

            foreach (Track track in tracks)
            {
                ListViewItem item = new ListViewItem(track.Name);                  
                item.SubItems.Add(track.Artist);                          
                item.SubItems.Add(Track.FormatDuration(track.Duration));       

                listView4.Items.Add(item);
            }
        }

        /// <summary>
        /// Accessible via Enter key to trigger search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchBtn_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Function to like or unlike the currently selected track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LikeSongBtn_Click(object sender, EventArgs e)
        {
            if (this.trackDetails == null)
            {
                MessageBox.Show("No track selected to like.",
                                "Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            Track.SaveTrackJson(this.trackDetails);
            if (Track.IsTrackSaved(this.trackDetails.Id))
            {
                LikeSongBtn.Text = "Unlike Song";
            }
            else
            {
                LikeSongBtn.Text = "Like Song";
            }
            MessageBox.Show("Successfully Liked/Unliked Track!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows the Liked Tracks form when the Liked button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LikedBtn_Click(object sender, EventArgs e)
        {
            LikedTracksForm likedTracksForm = new LikedTracksForm();
            likedTracksForm.ShowDialog();
        }
    }
}