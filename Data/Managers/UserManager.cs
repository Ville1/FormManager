using FormManager.Controllers;
using FormManager.Data.Models;

namespace FormManager.Data.Managers
{
    public class UserManager : ManagerBase<User>
    {
        public UserManager(Database database, ControllerBase? controller) : base(database, controller, database.Users) { }

        public bool CanLogIn(string email, string password)
        {
            return Exists(user => user.Email == email && user.Password == password);
        }

        public User GetLoginUser(string email, string password)
        {
            return Get(user => user.Email == email && user.Password == password);
        }

        public string GetUserName(Guid? id)
        {
            if(id == null) {
                return string.Empty;
            } else {
                return GetUserName(id.Value);
            }
        }

        public string GetUserName(Guid id)
        {
            if(id == Guid.Empty) {
                //No user id = system user
                return Resources.Localization.System;
            } else {
                if (Has(id)) {
                    User user = Get(id);
                    if(user.UserName == null || user.UserName == string.Empty) {
                        return string.Format("[{0}]", Resources.Localization.MissingUserName.Replace(" ", "_").ToUpper());
                    }
                    return user.UserName;
                } else {
                    return string.Format("{0}: {1}", Resources.Localization.DeletedUser, id.ToString());
                }
            }
        }
    }
}
