using DataAccess.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccess.DatabaseContext
{
    internal class DbContext : System.Data.Entity.DbContext
    {
        // Recommended:
        // Call base constructor with the name of the DB connection string in App.config.
        // This is not necessarily the name of the actual database.
        public DbContext() : base("Database") { }

        // Create DbSet methods for each table in db
        // Must correspond to entity classes in Model
        // Method signature: DbSet<[class entity]> [table entity] {}
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }

        /* protected override void OnModelCreating(DbModelBuilder modelBuilder)
         {
             modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
         }
        */

        // Make sure EntityFramework.SqlServer.dll is included in bin folder
        private void dummy()
        {
            string result = System.Data.Entity.SqlServer.SqlFunctions.Char(65);
        }


    }
}
