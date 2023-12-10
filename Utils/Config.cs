namespace FormManager.Utils
{
    public class Config
    {
        public static IConfiguration? Configuration { get; set; }

        public static string ConnectionString
        {
            get {
                return Configuration == null ? string.Empty : Configuration.GetConnectionString("Default") ?? string.Empty;
            }
        }
    }
}
