using FormManager.Controllers;
using FormManager.Data.Models.Forms;

namespace FormManager.Data.Managers
{
    public class DeveloperManager : ManagerBase<Developer>
    {
        public DeveloperManager(Database database, ControllerBase? controller) : base(database, controller, database.Developers, "VideoGames") { }
    }
}
