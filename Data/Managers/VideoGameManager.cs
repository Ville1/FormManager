using FormManager.Data.Models.Forms;

namespace FormManager.Data.Managers
{
    public class VideoGameManager : ManagerBase<VideoGame>
    {
        public VideoGameManager(Database database) : base(database, database.VideoGames) { }
    }
}
