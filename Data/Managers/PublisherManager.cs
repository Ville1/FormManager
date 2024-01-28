using FormManager.Controllers;
using FormManager.Data.Models.Forms;

namespace FormManager.Data.Managers
{
    public class PublisherManager : ManagerBase<Publisher>
    {
        public PublisherManager(Database database, ControllerBase? controller) : base(database, controller, database.Publishers, "VideoGames") { }
    }
}
