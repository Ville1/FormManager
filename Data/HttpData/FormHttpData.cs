namespace FormManager.Data.HttpData
{
    /// <summary>
    /// Base class for form requests and responses
    /// </summary>
    public class FormHttpData
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Created { get; set; }

        public bool CanBeDeleted { get; set; }
        public List<string> DeleteErrors { get; set; } = new List<string>();
    }
}
