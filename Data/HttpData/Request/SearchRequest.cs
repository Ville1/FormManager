namespace FormManager.Data.HttpData.Request
{
    public class SearchRequest
    {
        public long CurrentPage { get; set; }
        public long PageSize { get; set; }
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
    }
}
