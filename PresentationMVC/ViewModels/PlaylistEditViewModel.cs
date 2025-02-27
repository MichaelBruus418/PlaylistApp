using DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationMVC.ViewModels
{
    public class PlaylistEditViewModel
    {
        public Playlist Playlist { get; set; }
        public List<Track> TrackPool { get; set; }

        public PlaylistEditViewModel() { }

        public PlaylistEditViewModel(Playlist playlist, List<Track> trackPool)
        {
            Playlist = playlist;
            TrackPool = trackPool;
        }

    }
}
