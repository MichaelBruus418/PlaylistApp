using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappers
{
    internal class TrackMapper
    {
        // Maps DAL Track obj to DTO Track obj
        public static DTO.Model.Track Map (Track track)
        {
            //return new DTO.Model.Track(
                //track.Id, track.Title, track.Length, track.Playlists.ConvertAll(p => PlaylistMapper.Map(p))
            //); 


            // WORKAROUND WITHOUT PLAYLISTS MAPPING
            return new DTO.Model.Track(track.Id, track.Title, track.Length);
        }

        // Maps DTO Track obj to DAL Track obj
        public static Track Map (DTO.Model.Track track)
        {
            if (track.Id.HasValue)
            {
                //return new Track(track.Id.Value, track.Title, track.Length, track.Playlists.ConvertAll(p => PlaylistMapper.Map(p)));

                // WORKAROUND WITHOUD PLAYLISTS MAPPING
                return new Track(track.Id.Value, track.Title, track.Length);
            }
            else
            {
                return new Track(track.Title, track.Length, track.Playlists.ConvertAll(p => PlaylistMapper.Map(p)));
            }
        }
    }
}
