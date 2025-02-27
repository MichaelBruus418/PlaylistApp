using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Model
{
    public class Track
    {
        public int? Id { get; set; }

        //[Display(Name = "Titel")]
        //[Required(ErrorMessage = "Please enter title.")] // Overrides error message set in controller.
        //[Required] // Overrides error message set in controller.
        public string Title { get; set; }

        // DataType.Time reagerer forskelligt afhængigt af browser.
        // Derfor bruger vi ikke denne attribut.
        public TimeSpan? Length { get; set; }

        public List<Playlist> Playlists { get; set; } = new List<Playlist>();

        // Boolean var for generic use by GUI
        public Boolean IsSelected { get; set; } = false;

        // --- Methods --------------------------------------------

        // Empty construtor is necessary for entity framework
        public Track() {}

        public Track(int? id, string title) 
        {
            Id = id;
            Title = title;
        }

        public Track(int? id, string title, TimeSpan? length) : this(id, title)
        {
            Length = length;
        }

        public Track(int? id, string title, TimeSpan? length, List<Playlist> playlists) : this(id, title, length)
        {
            Playlists = playlists;
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
            return txt;
        }
    }
}
