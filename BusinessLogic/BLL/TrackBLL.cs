using DataAccess.Repositories;
using DTO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BLL
{
    public class TrackBLL
    {
        public List<DTO.Model.Track> GetAll()
        {
            return TrackRepository.GetAll();
        }

        // Returns list with all tracks where each tracks IsSelected() var
        // reflects if it is included in passed playlist.
        public List<Track> GetAll(Playlist pl)
        {
            List<Track> tracks = GetAll();

            foreach (Track plt in pl.Tracks)
            {
                tracks.Single(t => t.Id == plt.Id).IsSelected = true;
            }
            
            return tracks;
        }

        public Track GetById(int id)
        {
            if (id < 0) throw new IndexOutOfRangeException();
            return TrackRepository.GetTrackById(id);
        }

        public List<Track> GetTracksById(List<int> ids)
        {
            return TrackRepository.GetTracksById(ids);
        }

        public void AddTrack(Track track)
        {
            if (String.IsNullOrEmpty(track.Title)) throw new InvalidOperationException("Title may not be null or empty."); 
            TrackRepository.AddTrack(track);
        }

        public void UpdateTrack(Track track)
        {
            TrackRepository.UpdateTrack(track);
        }

        public int DeleteTrack(Track track)
        {
            return TrackRepository.DeleteTrack(track);
        }

        /**
         * @returns num of rows deleted
         */
        public int DeleteTracks(List<Track> tracks)
        {
            return TrackRepository.DeleteTracks(tracks);
        }

        public int DeleteById(int id)
        {
            return TrackRepository.DeleteTrackById(id);
        }

        public int DeleteByIds(List<int> ids)
        {
            return TrackRepository.DeleteTracksById(ids);
        }

    }
}
