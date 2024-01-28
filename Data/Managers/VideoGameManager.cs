using FormManager.Controllers;
using FormManager.Data.Models.Forms;

namespace FormManager.Data.Managers
{
    public class VideoGameManager : ManagerBase<VideoGame>
    {
        public VideoGameManager(Database database, ControllerBase? controller) : base(database, controller, database.VideoGames, new List<string>() { "Developer", "Publisher" }) { }
    }
}
