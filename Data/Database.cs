using FormManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
