using FormManager.Controllers;
using FormManager.Data;
using FormManager.Data.Managers;

namespace FormManager.Services
{
    public class DatabaseService
    {
        private readonly Database context;

        public ControllerBase? Controller { get; set; }

        public DatabaseService(Database context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private UserManager? users;
        public UserManager Users
        {
            get {
                if(users == null) {
                    users = new UserManager(context, Controller);
                }
                return users;
            }
        }

        private LogManager? log;
        public LogManager Log
        {
            get {
                if (log == null) {
                    log = new LogManager(context, Controller);
                }
                return log;
            }
        }

        private VideoGameManager? videoGames;
        public VideoGameManager VideoGames
        {
            get {
                if (videoGames == null) {
                    videoGames = new VideoGameManager(context, Controller);
                }
                return videoGames;
            }
        }

        private DeveloperManager? developers;
        public DeveloperManager Developers
        {
            get {
                if (developers == null) {
                    developers = new DeveloperManager(context, Controller);
                }
                return developers;
            }
        }

        private PublisherManager? publishers;
        public PublisherManager Publishers
        {
            get {
                if (publishers == null) {
                    publishers = new PublisherManager(context, Controller);
                }
                return publishers;
            }
        }
    }
}
