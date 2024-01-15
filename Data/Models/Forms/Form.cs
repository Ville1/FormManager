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

            List<string> deleteErrors;
            data.CanBeDeleted = CanBeDeleted(out deleteErrors);
            data.DeleteErrors = deleteErrors;
        }

        public virtual bool CanBeDeleted(out List<string> errors)
        {
            errors = new List<string>();
            return true;
        }
    }
}
