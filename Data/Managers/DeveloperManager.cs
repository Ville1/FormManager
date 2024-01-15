using FormManager.Data.Models.Forms;

namespace FormManager.Data.Managers
{
    public class DeveloperManager : ManagerBase<Developer>
    {
        public DeveloperManager(Database database) : base(database, database.Developers, "VideoGames") { }
    }
}
