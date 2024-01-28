namespace FormManager.Data.Models.Log
{
    public class LogParameter : IDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid LogId { get; set; }
        public Log Log { get; set; } = null!;
        public string Key { get; set; } = string.Empty;
        public string? Value { get; set; } = null;

        public static LogParameterKeys Keys { get { return new LogParameterKeys(); } }

        public class LogParameterKeys
        {
            public string FormId = "FormId";
            public string OldValue = "OldValue";
            public string NewValue = "NewValue";
        }
    }
}
