using FormManager.Data.Models.Forms;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Data.Managers
{
    public class VideoGameManager : ManagerBase<VideoGame>
    {
        public VideoGameManager(Database database) : base(database, database.VideoGames) { }

        public override IEnumerable<VideoGame> Search(Func<VideoGame, bool> searchFunc)
        {
            return dbSet
                .Include(game => game.Developer)
                .Include(game => game.Publisher)
                .Where(searchFunc);
        }
    }
}
