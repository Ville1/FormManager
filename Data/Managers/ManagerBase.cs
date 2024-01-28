using FormManager.Controllers;
using FormManager.Data.Models;
using FormManager.Data.Models.Forms;
using FormManager.Data.Models.Log;
using FormManager.Data.Models.Log.Events;
using FormManager.Utils;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FormManager.Data.Managers
{
    public class ManagerBase<TModel> where TModel : class, IDatabaseModel
    {
        protected Database database;
        protected DbSet<TModel> dbSet;
        protected List<string> defaultIncludePaths;
        protected bool logEvents;
        protected ControllerBase? controller;

        public ManagerBase(Database database, ControllerBase? controller, DbSet<TModel> dbSet, List<string> defaultIncludePaths, bool logEvents = true)
        {
            this.database = database;
            this.controller = controller;
            this.dbSet = dbSet;
            this.defaultIncludePaths = defaultIncludePaths.Select(x => x).ToList();
            this.logEvents = logEvents;
        }

        public ManagerBase(Database database, ControllerBase? controller, DbSet<TModel> dbSet, string defaultIncludePath, bool logEvents = true)
        {
            this.database = database;
            this.controller = controller;
            this.dbSet = dbSet;
            defaultIncludePaths = new List<string>() { defaultIncludePath };
            this.logEvents = logEvents;
        }

        public ManagerBase(Database database, ControllerBase? controller, DbSet<TModel> dbSet, bool logEvents = true)
        {
            this.database = database;
            this.controller = controller;
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
            //Get item
            TModel item;
            if(defaultIncludePaths.Count == 0) {
                //No default include
                item = dbSet.First(x => x.Id == id);
            } else {
                //Include properties from defaultIncludePaths
                IQueryable<TModel> query = dbSet.AsQueryable();
                foreach(string path in defaultIncludePaths) {
                    query = query.Include(path);
                }
                item = query.First(x => x.Id == id);
            }

            if(item is Form form) {
                //This is a form, include log
                form.Log = GetFormLog(form).ToList();
            }

            return item;
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
            if (newItem is Form form) {
                //This is a form
                form.Created = Logging.TimeStamp;
                if(UserIsLoggedIn) {
                    //User is logged in
                    form.CreatorId = UserId;
                } else {
                    //System user
                    form.CreatorId = Guid.Empty;
                }
            }

            //Add to db
            dbSet.Add(newItem);

            //Save changes
            database.SaveChanges();
            return newItem.Id;
        }

        public bool Save(TModel newData, List<string>? saveProperties = null)
        {
            if (!Has(newData.Id)) {
                //Item does not exist
                return false;
            }

            //Get old data
            TModel item = dbSet.First(x => x.Id == newData.Id);

            //Make changes
            //Loop through all properties
            foreach(PropertyInfo property in item.GetType().GetProperties()) {
                //Check if this should save changes in this property
                if(property.CanWrite && (saveProperties == null || saveProperties.Contains(property.Name))) {
                    //Check if there is a change in this property
                    object? oldValue = property.GetValue(item);
                    object? newValue = property.GetValue(newData);
                    if((oldValue == null && newValue != null) ||//null -> not null
                        (oldValue != null && newValue == null) ||//not null -> null
                        (oldValue != null && newValue != null && !oldValue.Equals(newValue))) {//Both are not null, but value has changed
                        //Set new value
                        property.SetValue(item, newValue);

                        if (item is Form form) {
                            //This is a form, log change
                            database.Logs.Add(new FormChangedEvent(UserId, item.Id, property.Name, oldValue?.ToString(), newValue?.ToString()));
                        }
                    }
                }
            }

            //Save changes
            database.SaveChanges();

            return true;
        }

        public void Delete(TModel item)
        {
            //Remove item
            dbSet.Remove(item);

            if (item is Form form) {
                //This is a form, delete log
                IEnumerable<Log> formLog = GetFormLog(form);
                foreach(Log log in formLog) {
                    foreach(LogParameter parameter in log.Parameters) {
                        database.LogParameters.Remove(parameter);
                    }
                    database.Logs.Remove(log);
                }
            }

            //Save changes
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

        protected bool UserIsLoggedIn
        {
            get {
                return controller != null && !controller.IsAnonymous;
            }
        }

        protected Guid UserId
        {
            get {
                return controller != null && !controller.IsAnonymous ? controller.User.Id : Guid.Empty;
            }
        }

        private IEnumerable<Log> GetFormLog(Form form)
        {
            List<LogEventType> formEvents = new List<LogEventType>() { LogEventType.FormChanged };
            return database.Logs.Include(log => log.Parameters).Where(log =>
                formEvents.Contains(log.Type) &&
                log.Parameters.Any(parameter => parameter.Key == LogParameter.Keys.FormId && parameter.Value == form.Id.ToString())
            ).OrderByDescending(log => log.TimeStamp);
        }
    }
}
