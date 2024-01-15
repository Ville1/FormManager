using FormManager.Data.Models.Forms;

namespace FormManager.Data.Managers
{
    public class PublisherManager : ManagerBase<Publisher>
    {
        public PublisherManager(Database database) : base(database, database.Publishers, "VideoGames") { }
    }
}
