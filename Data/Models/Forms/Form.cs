using FormManager.Data.HttpData;

namespace FormManager.Data.Models.Forms
{
    public class Form : IDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Created { get; set; }

        public void SetHttpData(FormHttpData data)
        {
            data.Id = Id;
            data.CreatorId = CreatorId;
            data.Created = Created;
        }
    }
}
