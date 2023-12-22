namespace FormManager.Data.Models.Forms
{
    public class Form : IDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Created { get; set; }
    }
}
