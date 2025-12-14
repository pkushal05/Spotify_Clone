using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spotify_Clone
{
    /// <summary>
    /// A UserControl representing an album card with cover image and name.
    /// </summary>
    public partial class AlbumCard : UserControl
    {
        // Unique identifier for the album
        public string ID;
        public AlbumCard()
        {
            InitializeComponent();

            // Subscribe to user control events to propagate clicks
            CoverImage.Click += (s, e) => this.OnClick(e);
            NameLabel.Click += (s, e) => this.OnClick(e);
        }

        // Change background color on mouse enter and leave
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.LightGray;
        }

        // Change background color back on mouse leave
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
        }

        /// <summary>
        /// Handles disposal of images to free up resources.
        /// </summary>
        public void DisposeImages()
        {
            if (CoverImage.Image != null)
            {
                CoverImage.Image.Dispose();
                CoverImage.Image = null;
            }
        }

        /// <summary>
        /// Function to set the album data on the card.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="albumName"></param>
        /// <param name="albumArt"></param>
        public void SetData(string ID, string albumName, Image albumArt)
        {
            DisposeImages();
            this.ID = ID;
            NameLabel.Text = albumName;
            CoverImage.Image = albumArt;
        }
    }
}
