namespace FormManager.Data.HttpData.Response
{
    public class LogResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
