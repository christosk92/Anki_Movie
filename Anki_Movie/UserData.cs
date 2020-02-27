using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiMovie_Console_Test
{
    public class UserData
    {
        public List<ArtItem> ArtItems { get; set; }
    }
    public class ArtItem
    {
        public ArtItem(string MoviePath, string SrtPath)
        {
            this.MoviePath = MoviePath;
            this.SrtPath = SrtPath;
            this.DateAdded = DateTime.UtcNow;
        }
        public string MoviePath { get; set; }
        public string SrtPath { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
