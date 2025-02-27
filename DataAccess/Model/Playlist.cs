using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.Model
{
    internal class Playlist
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Track> Tracks { get; set; } = new List<Track>();

        // --------------------

        public Playlist() { }

        public Playlist(string name)
        {
            Name = name;
        }

        public Playlist(string name, List<Track> tracks) : this(name)
        {
            Tracks = tracks;
        }

        public Playlist(int id, string name, List<Track> tracks) : this(name, tracks)
        {
            Id = id;
        }

        public override String ToString()
        {
            String txt = base.ToString() + "\n";
            txt += "Id: " + this.Id + "\n";
            txt += "Name: " + this.Name + "\n";
            txt += "Num of Tracks: " + this.Tracks.Count() + "\n";
            return txt;
        }

        public void AddTrack(Track t)
        {
            Tracks.Add(t);
        }

        public void RemoveTrack(Track t)
        {
            Tracks.Remove(t);
        }
    }
}

