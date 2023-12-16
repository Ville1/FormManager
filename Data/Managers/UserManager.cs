using FormManager.Data.Models;

namespace FormManager.Data.Managers
{
    public class UserManager : ManagerBase<User>
    {
        public UserManager(Database database) : base(database, database.Users) { }

        public bool CanLogIn(string email, string password)
        {
            return Exists(user => user.Email == email && user.Password == password);
        }

        public User GetLoginUser(string email, string password)
        {
            return Get(user => user.Email == email && user.Password == password);
        }
    }
}
