using DataAccess.Model;
using DataAccess.DatabaseContext;
using System.Data.Entity;
using System;
using System.Collections.Generic;

namespace DataAccess.DatabaseContext
{
    // Initialzer class must inherit from an appropriate parent
    // Several options are available (e.g.):
    // CreateDatabaseIfNotExists<[databaseContextClassName]>
    // DropCreateDatabaseIfModelChanges<[databaseContextClassName]>
    // DropCreateDatabaseAlways<DbContext>
    // Custom DB initializer (make your own scheme)
    internal class DbInitializer : DropCreateDatabaseIfModelChanges<DbContext>
    {
        // Initial population of database
        // @param [databaseContextClassName] context
        protected override void Seed(DbContext context)
        {
            // --- Tracks table ---
            List<Track> tracks = new List<Track>();
            tracks.Add(new Track("Bad Romance", new TimeSpan(0, 4, 33)));
            tracks.Add(new Track("Poker Face"));
            tracks.Add(new Track("Telephone"));
            tracks.Add(new Track("Bed of Roses", new TimeSpan(0, 14, 33)));
            tracks.Add(new Track("Benzin", new TimeSpan(0, 114, 33)));
            tracks.Add(new Track("Deutschland", new TimeSpan(0, 64, 33)));
            tracks.Add(new Track("Rock Your Body", new TimeSpan(0, 69, 99)));
            tracks.Add(new Track("America", new TimeSpan(0, 1, 23)));

            foreach (Track track in tracks)
            {
                context.Tracks.Add(track);
            }

            // --- Playlists table ---
            List<Playlist> playllists = new List<Playlist>();
            playllists.Add(new Playlist("Mixed goodies"));
            playllists.Add(new Playlist("Rock Your Body"));
            playllists.Add(new Playlist("Rammstein only"));

            foreach (Playlist pl in playllists)
            {
                context.Playlists.Add(pl);
            }

            context.SaveChanges();
            
            // - Populate playlists with some tracks -
            // Mixed goodies
            playllists[0].AddTrack(tracks[0]);
            playllists[0].AddTrack(tracks[3]);
            playllists[0].AddTrack(tracks[5]);

            // Rock Your Body
            playllists[1].AddTrack(tracks[6]);
            playllists[1].AddTrack(tracks[6]);
            playllists[1].AddTrack(tracks[6]);
            playllists[1].AddTrack(tracks[6]);

            // Rammstein only
            playllists[2].AddTrack(tracks[5]);
            playllists[2].AddTrack(tracks[4]);
            playllists[2].AddTrack(tracks[7]);

            // --- Done ---
            context.SaveChanges();

        }
    }
}
