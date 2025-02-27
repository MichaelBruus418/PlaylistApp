using DataAccess.DatabaseContext;
using DataAccess.Mappers;
using DataAccess.Model;
using DTO.Model;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext = DataAccess.DatabaseContext.DbContext;
using Playlist = DataAccess.Model.Playlist;
using Track = DataAccess.Model.Track;

namespace DataAccess.Repositories
{
    public class PlaylistRepository
    {
        public static List<DTO.Model.Playlist> GetAll()
        {
            using (DbContext context = new DbContext())
            {
                // Read without related data
                var result = context.Playlists;

                // Read with related data
                //var* result = context.Playlists.Include(p => p.Tracks);
                
                List<Playlist> playlists = result.ToList<Playlist>();
                return playlists.ConvertAll(pl => PlaylistMapper.Map(pl));
            }
        }

        public static DTO.Model.Playlist GetById(int id)
        {
            using (DbContext context = new DbContext())
            {
                // Read with related data 
                var result = context.Playlists.Where(p => p.Id == id).Include(p => p.Tracks);

               Playlist pl = result.First();

                // Convert result to List and take first element.
                //Playlist pl = result.ToList<Playlist>()[0];
                //Playlist pl = result.ToList<Playlist>().First();

                return PlaylistMapper.Map(pl); 
            }
        }

        public static void AddPlaylist(DTO.Model.Playlist pl)
        {
            using (DbContext context = new DbContext())
            {
                context.Playlists.Add(PlaylistMapper.Map(pl));
                context.SaveChanges();
            }
        }

        public static void Update(DTO.Model.Playlist playlistDTO)
        {
            if (playlistDTO.Id.HasValue)
            {
                using (DbContext context = new DbContext())
                {
                    try
                    {
                        // NOTE:
                        // Nedenstående er en workaround, da vi ikke har lært hvordan man behandler
                        // many-to-many relationer mht. DTO objekter og updates.
                        
                        // Get all track ids from DTO playlist
                        List<int> trackIds = playlistDTO.Tracks.Select(x => (int)x.Id).ToList();
                        // Read these tracks from DB
                        List<Track> tracks = context.Tracks.Where(t => trackIds.Contains(t.Id)).ToList();

                        // Use playlistDTO id to read related DTA playlist. 
                        var result = context.Playlists.Where(p => p.Id == playlistDTO.Id.Value).Include(p => p.Tracks);
                        // Convert result to List and take first element.
                        Playlist pl = result.ToList<Playlist>()[0];

                        // Set tracks
                        pl.Tracks = tracks;

                        // Set as Modified
                        context.Entry(pl).State = System.Data.Entity.EntityState.Modified;
                        int numOfRowsUpdated = context.SaveChanges();
                        Trace.WriteLine("NumOfRowsUpdated: " + numOfRowsUpdated);
                    }
                    catch
                    {
                        throw new ArgumentException("Unable to update. Playlist id doesn't exist in DB.");
                    }
                }

            }
            else
            {
                throw new InvalidOperationException("Unable to update. Id of Playlist is null.");
            }
        }

        public static int DeletePlaylistById(int id)
        {
            using (DbContext context = new DbContext())
            {
                context.Entry(new Playlist() { Id = id }).State = System.Data.Entity.EntityState.Deleted;
                return context.SaveChanges(); // Returns num of rows deleted.
            }
        }
    }
}
