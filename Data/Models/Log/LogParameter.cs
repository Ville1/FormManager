namespace FormManager.Data.Models.Log
{
    public class LogParameter : IDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid LogId { get; set; }
        public Log Log { get; set; } = null!;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
