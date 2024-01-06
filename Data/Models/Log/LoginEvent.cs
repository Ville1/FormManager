using FormManager.Data.Managers;

namespace FormManager.Data.Models.Log
{
    public class LoginEvent : Log
    {
        public LoginEvent(Guid userId)
        {
            Type = LogEventType.Login;
            UserId = userId;
            TimeStamp = LogManager.TimeStamp;
        }
    }
}
