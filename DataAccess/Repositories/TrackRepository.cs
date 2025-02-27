using DataAccess.DatabaseContext;
using DataAccess.Mappers;
using DataAccess.Model;
using DTO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using Track = DataAccess.Model.Track;

namespace DataAccess.Repositories
{
    public class TrackRepository
    {
        public static List<DTO.Model.Track> GetAll()
        {
            using (DbContext context = new DbContext())
            {
                // --- Method 1 ---
                //List<Track> trackList = context.Tracks
                //    .SqlQuery("Select * from Tracks")
                //    .ToList<Track>();


                // --- Method 2 ---
                //var result = context.Tracks;

                // --- Method 3 ---
                var result = from t in context.Tracks
                             orderby t.Title
                             select t;

                List<Track> trackList = result.ToList<Track>();
                return trackList.ConvertAll(t => TrackMapper.Map(t));
            }
        }
        
        public static DTO.Model.Track GetTrackById(int id)
        {
            // The using-block causes objects initialized within it to be disposed (cleared in memory),
            // after it is excuted, even if exception is thrown.
            using (DbContext context = new DbContext())
            {
                try
                {
                    return TrackMapper.Map(context.Tracks.Find(id));
                }
                catch (NullReferenceException e)
                {
                    throw new ArgumentException("Track id doesn't exist.");
                }
            }
        }

        public static List<DTO.Model.Track> GetTracksById(List<int> ids)
        {
            using (DbContext context = new DbContext())
            {

                // --- Method 1 ---
                //IQueryable<Track> result = from t in context.Tracks
                //                           where ids.Contains(t.Id)
                //                           select t;
                //List<Track> tracks = result.ToList();

                // --- Method 2 ---
                List<Track> tracks = context.Tracks.Where(t => ids.Contains(t.Id)).ToList();

                return tracks.ConvertAll(t => TrackMapper.Map(t));
            }
        }

        public static void AddTrack(DTO.Model.Track track)
        {
            using (DbContext context = new DbContext())
            {
                context.Tracks.Add(TrackMapper.Map(track));
                int rowsChanged = context.SaveChanges();
                Trace.WriteLine("Rows Changed: " + rowsChanged);
            }
        }

        public static void UpdateTrack(DTO.Model.Track track)
        {
            if (track.Id != null)
            {
                using (DbContext context = new DbContext())
                {
                    // Convert to DAL obj
                    Model.Track t = TrackMapper.Map(track);
                    // Set as Modified
                    context.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

            }
            else
            {
                throw new InvalidOperationException("Unable to update. Id of Track is null.");
            }
        }

        public static int DeleteTrack(DTO.Model.Track track)
        {
            List<DTO.Model.Track> tracks = new List<DTO.Model.Track>();
            tracks.Add(track);
            return DeleteTracks(tracks);
        }

        /**
         * Note:
         * Passed Track objects are only required to be populated with Id.
        * @returns num of rows deleted
        */
        public static int DeleteTracks(List<DTO.Model.Track> tracks)
        {
            using (DbContext context = new DbContext())
            {
                foreach (DTO.Model.Track track in tracks)
                {
                    // If track.id is null, we regard it as deleted.
                    if (track.Id != null)
                    {
                        context.Entry(TrackMapper.Map(track)).State = System.Data.Entity.EntityState.Deleted;
                    }
                    else
                    {
                        Trace.WriteLine("Track has no Id. Track title: " + track.Title);
                    }
                }

                return context.SaveChanges(); // Returns num of rows deleted.
            }

        }


        public static int DeleteTrackById(int id)
        {
            List<int> ids = new List<int>();
            ids.Add(id);
            return DeleteTracksById(ids);
        }

        public static int DeleteTracksById(List<int> ids)
        {
            // --- Method 1: Using raw SQL ---
            //using (DbContext context = new DbContext())
            //{
            //    int numRowsDeleted = context.Database.ExecuteSqlCommand(
            //        "DELETE FROM Tracks WHERE Id IN(" + string.Join(", ", ids) + ")"
            //    );

            //    return numRowsDeleted;
            //}

            // --- Method 2: Using Entity State (DeleteTracks() method) ---
            // Create list of tracks where we only set Id.
            List<DTO.Model.Track> tracks = new List<DTO.Model.Track>();
            foreach (int id in ids)
            {
                tracks.Add(new DTO.Model.Track(){ Id = id});
            }
            
            return DeleteTracks(tracks);

        }
            
       
    }
}
