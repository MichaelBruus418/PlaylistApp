using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Model
{
    public class Playlist
    {
        public int? Id { get; set; }

        //[Display(Name = "Titel")]
        //[Required(ErrorMessage = "Please enter name.")] // Overrides error message set in controller.
        //[Required] // Overrides error message set in controller.
        public string Name { get; set; }

        public List<Track> Tracks { get; set; } = new List<Track>();

        // Empty construtor is necessary for entity framework
        public Playlist() { }

        public Playlist(int? id, string name, List<Track> tracks)
        {
            Id = id;
            Name = name;
            Tracks = tracks;
        }

        public override String ToString()
        {
            String txt = base.ToString() + "\n";
            txt += "Id: " + this.Id + "\n";
            txt += "Name: " + this.Name + "\n";
            return txt;
        }
    }
}
