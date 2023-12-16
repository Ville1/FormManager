using FormManager.Data.Models;
using FormManager.Data.Models.Log;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<LogParameter> LogParameters => Set<LogParameter>();
    }
}
