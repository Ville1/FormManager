using FormManager.Data.HttpData;
using FormManager.Data.HttpData.Response;
using FormManager.Data.Models.Log;
using FormManager.Services;

namespace FormManager.Data.Models.Forms
{
    public class Form : IDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Created { get; set; }

        public List<Log.Log> Log { get; set; } = new List<Log.Log>();
        public Guid? EditorId { get { return Log.Count == 0 ? null : Log[0].UserId; } }
        public DateTime? Edited { get { return Log.Count == 0 ? null : Log[0].TimeStamp; } }

        public void SetHttpData(FormHttpData data, DatabaseService database)
        {
            data.Id = Id;
            data.CreatorId = CreatorId;
            data.Created = Created;
            data.CreatorName = database.Users.GetUserName(data.CreatorId);
            data.Edited = Edited;
            data.EditorId = EditorId;
            data.EditorName = database.Users.GetUserName(data.EditorId);

            List<string> deleteErrors;
            data.CanBeDeleted = CanBeDeleted(out deleteErrors);
            data.DeleteErrors = deleteErrors;
        }

        public void SetHttpResponseLogData(FormHttpResponseData data, DatabaseService database)
        {
            data.Log = Log.Select(log => {
                LogResponse response = new LogResponse() {
                    Id = log.Id,
                    Type = log.Type.ToString(),
                    UserId = log.UserId,
                    UserName = database.Users.GetUserName(log.UserId),
                    TimeStamp = log.TimeStamp,
                    Text = log.Type.ToString()
                };
                switch (log.Type) {
                    case LogEventType.FormChanged:
                        LogParameter? propertyName = log.Parameters.FirstOrDefault(parameter => parameter.Key == LogParameter.Keys.PropertyName);
                        LogParameter? oldValue = log.Parameters.FirstOrDefault(parameter => parameter.Key == LogParameter.Keys.OldValue);
                        LogParameter? newValue = log.Parameters.FirstOrDefault(parameter => parameter.Key == LogParameter.Keys.NewValue);
                        response.Text = string.Format("[{0}]: \"{1}\" -> \"{2}\"", propertyName?.Value?.ToLower() ?? "???", oldValue?.Value ?? string.Empty, newValue?.Value ?? string.Empty);
                        break;
                }
                return response;
            }).ToList();
        }

        public virtual bool CanBeDeleted(out List<string> errors)
        {
            errors = new List<string>();
            return true;
        }
    }
}
