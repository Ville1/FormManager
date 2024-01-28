namespace FormManager.Data.Models.Log
{
    public class Log : IDatabaseModel
    {
        public Guid Id { get; set; }
        public LogEventType Type { get; set; }
        /// <summary>
        /// Guid.Empty = system. Can also be an id of a deleted user
        /// </summary>
        public Guid UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public ICollection<LogParameter> Parameters { get; set; } = new List<LogParameter>();
    }
}
