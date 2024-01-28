using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;
using FormManager.Services;

namespace FormManager.Data.Models.Forms
{
    public class Developer : Form
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoGame> VideoGames { get; set; } = new List<VideoGame>();

        public DeveloperResponseData ToResponse(DatabaseService database)
        {
            DeveloperResponseData response = new DeveloperResponseData();
            SetHttpData(response, database);
            SetHttpResponseLogData(response, database);
            response.Name = Name;
            return response;
        }

        public static Developer FromRequestData(DeveloperRequestData requestData)
        {
            return new Developer {
                Id = requestData.Id,
                Name = requestData.Name
            };
        }

        public override bool CanBeDeleted(out List<string> errors)
        {
            errors = new List<string>();
            if(VideoGames.Count == 0) {
                return true;
            }
            foreach(VideoGame videoGame in VideoGames) {
                errors.Add(Resources.Localization.DeleteErrorMessageUsedBy.Replace("[type]", Resources.Localization.VideoGame.ToLower()).Replace("[name]", videoGame.Name));
            }
            return false;
        }
    }
}
