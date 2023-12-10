using FormManager.Data;
using FormManager.Data.Managers;

namespace FormManager.Services
{
    public class DatabaseService
    {
        private readonly Database context;

        public DatabaseService(Database context)
        {
            this.context = context;
        }

        public UserManager Users
        {
            get {
                return new UserManager(context);
            }
        }
    }
}
