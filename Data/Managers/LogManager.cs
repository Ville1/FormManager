using FormManager.Data.Models.Log;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data.Managers
{
    public class LogManager : ManagerBase<Log>
    {
        private DbSet<LogParameter> parameterDbSet;

        public LogManager(Database database) : base(database, database.Logs, false)
        {
            parameterDbSet = database.LogParameters;
        }

        public void Write(Log log)
        {
            Add(log);
        }

        public static DateTime TimeStamp
        {
            get {
                //This can be changed to DateTime.UtcNow, if we don't want to use local time
                return DateTime.Now;
            }
        }
    }
}
