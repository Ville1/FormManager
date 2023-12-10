using FormManager.Data.Models;

namespace FormManager.Data.Managers
{
    public class UserManager
    {
        private readonly Database database;
        private readonly ManagerBase<User> manager;


        public UserManager(Database database)
        {
            this.database = database;
            manager = new ManagerBase<User>(database.Users);
        }

        public User? Get(Guid id)
        {
            return manager.Get(id);
        }

        public bool CanLogIn(string email, string password)
        {
            return database.Users.Any(user => user.Email == email && user.Password == password);
        }

        public User GetLoginUser(string email, string password)
        {
            return database.Users.First(user => user.Email == email && user.Password == password);
        }
    }
}
