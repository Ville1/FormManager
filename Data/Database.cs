using FormManager.Data.Models;
using FormManager.Data.Models.Forms;
using FormManager.Data.Models.Log;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Use table-per-type mapping for inheritance
            modelBuilder.Entity<Form>().ToTable("Forms");
            modelBuilder.Entity<VideoGame>().ToTable("VideoGames");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<LogParameter> LogParameters => Set<LogParameter>();
        public DbSet<Form> Forms => Set<Form>();
        public DbSet<VideoGame> VideoGames => Set<VideoGame>();
    }
}
