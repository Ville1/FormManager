using FormManager.Controllers;
using FormManager.Data.Models.Log;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data.Managers
{
    public class LogManager : ManagerBase<Log>
    {
        private DbSet<LogParameter> parameterDbSet;

        public LogManager(Database database, ControllerBase? controller) : base(database, controller, database.Logs, false)
        {
            parameterDbSet = database.LogParameters;
        }

        /// <summary>
        /// Add log and save changes
        /// </summary>
        public void Write(Log log)
        {
            Add(log);
        }
    }
}
