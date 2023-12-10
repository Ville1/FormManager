using FormManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data.Managers
{
    public class ManagerBase<TModel> where TModel : class, IDatabaseModel
    {
        private DbSet<TModel> dbSet;

        public ManagerBase(DbSet<TModel> dbSet)
        {
            this.dbSet = dbSet;
        }

        public TModel? Get(Guid id)
        {
            return dbSet.FirstOrDefault(x => x.Id == id);
        }
    }
}
