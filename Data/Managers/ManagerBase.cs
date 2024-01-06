using FormManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data.Managers
{
    public class ManagerBase<TModel> where TModel : class, IDatabaseModel
    {
        protected Database database;
        protected DbSet<TModel> dbSet;
        protected bool logEvents;

        public ManagerBase(Database database, DbSet<TModel> dbSet, bool logEvents = true)
        {
            this.database = database;
            this.dbSet = dbSet;
            this.logEvents = logEvents;
        }

        public bool Has(Guid id)
        {
            return dbSet.Any(x => x.Id == id);
        }

        public TModel Get(Guid id)
        {
            return dbSet.First(x => x.Id == id);
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

        public IEnumerable<TModel> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public virtual IEnumerable<TModel> Search(Func<TModel, bool> searchFunc)
        {
            return dbSet.Where(searchFunc);
        }
    }
}
