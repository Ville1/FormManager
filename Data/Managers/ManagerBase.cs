using FormManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data.Managers
{
    public class ManagerBase<TModel> where TModel : class, IDatabaseModel
    {
        protected Database database;
        protected DbSet<TModel> dbSet;
        protected List<string> defaultIncludePaths;
        protected bool logEvents;

        public ManagerBase(Database database, DbSet<TModel> dbSet, List<string> defaultIncludePaths, bool logEvents = true)
        {
            this.database = database;
            this.dbSet = dbSet;
            this.defaultIncludePaths = defaultIncludePaths.Select(x => x).ToList();
            this.logEvents = logEvents;
        }

        public ManagerBase(Database database, DbSet<TModel> dbSet, string defaultIncludePath, bool logEvents = true)
        {
            this.database = database;
            this.dbSet = dbSet;
            defaultIncludePaths = new List<string>() { defaultIncludePath };
            this.logEvents = logEvents;
        }

        public ManagerBase(Database database, DbSet<TModel> dbSet, bool logEvents = true)
        {
            this.database = database;
            this.dbSet = dbSet;
            defaultIncludePaths = new List<string>();
            this.logEvents = logEvents;
        }

        public bool Has(Guid id)
        {
            return dbSet.Any(x => x.Id == id);
        }

        public TModel Get(Guid id)
        {
            if(defaultIncludePaths.Count == 0) {
                return dbSet.First(x => x.Id == id);
            } else {
                IQueryable<TModel> query = dbSet.AsQueryable();
                foreach(string path in defaultIncludePaths) {
                    query = query.Include(path);
                }
                return query.First(x => x.Id == id);
            }
        }

        public bool Exists(Func<TModel, bool> searchFunction)
        {
            return dbSet.Any(searchFunction);
        }

        public TModel Get(Func<TModel, bool> searchFunction)
        {
            return dbSet.First(searchFunction);
        }

        public Guid Add(TModel newItem)
        {
            dbSet.Add(newItem);
            database.SaveChanges();
            return newItem.Id;
        }

        public void Delete(TModel item)
        {
            dbSet.Remove(item);
            database.SaveChanges();
        }

        public IEnumerable<TModel> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public IEnumerable<Guid> GetAllIds()
        {
            return dbSet.Select(x => x.Id).AsEnumerable();
        }

        public IEnumerable<TModel> Search(Func<TModel, bool> searchFunc)
        {
            if (defaultIncludePaths.Count == 0) {
                return dbSet.Where(searchFunc);
            } else {
                IQueryable<TModel> query = dbSet.AsQueryable();
                foreach (string path in defaultIncludePaths) {
                    query = query.Include(path);
                }
                return query.Where(searchFunc);
            }
        }
    }
}
