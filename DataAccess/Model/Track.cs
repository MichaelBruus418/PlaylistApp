using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    internal class Track
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public TimeSpan? Length { get; set; }

        public virtual List<Playlist> Playlists { get; set; } = new List<Playlist>();
        //public virtual ICollection<Playlist> Playlists { get; set; } = new HashSet<Playlist>();

        // ----------------

        public Track() { }

        public Track(string title)
        {
            Title = title;
        }

        public Track(string title, TimeSpan? length) : this(title)
        {
            Length = length;
        }
        public Track(int id, string title, TimeSpan? length) : this(title, length)
        {
            Id = id;
        }

        public Track(string title, TimeSpan? length, List<Playlist> playlists) : this(title, length)
        {
            Playlists = playlists;
        }


        public Track(int id, string title, TimeSpan? length, List<Playlist> playlists) : this(title, length, playlists)
        {
            Id = id;
        }

        public override String ToString()
        {
            String txt = base.ToString() + "\n";
            txt += "Id: " + this.Id + "\n";
            txt += "Name: " + this.Title + "\n";
            txt += "Length: ";
            if (this.Length.HasValue)
            {
                txt += " (" + ((TimeSpan)this.Length).ToString(@"mm\:ss") + ")";
            }
            txt += "\nNum of assicated playlists: " + this.Playlists.Count + "\n";
            return txt;
        }
    }
}
