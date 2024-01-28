namespace FormManager.Utils
{
    public class Logging
    {
        public static DateTime TimeStamp
        {
            get {
                //This can be changed to DateTime.UtcNow, if we don't want to use local time
                return DateTime.Now;
            }
        }
    }
}
