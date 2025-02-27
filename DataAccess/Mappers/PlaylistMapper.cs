using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;

namespace DataAccess.Mappers
{
    internal class PlaylistMapper
    {
        // Maps DAL Track obj to DTO Track obj
        public static DTO.Model.Playlist Map(Playlist pl)
        {
            return new DTO.Model.Playlist(pl.Id, pl.Name, pl.Tracks.ConvertAll(t => TrackMapper.Map(t)));
        }

        // Maps DTO Playlist obj to DAL Playlist obj
        public static Playlist Map(DTO.Model.Playlist pl)
        {
            if (pl.Id.HasValue)
            {
                return new Playlist(pl.Id.Value, pl.Name, pl.Tracks.ConvertAll(t => TrackMapper.Map(t)));
            }
            else
            {
                return new Playlist(pl.Name, pl.Tracks.ConvertAll(t => TrackMapper.Map(t)));
            }
        }
    }
}
