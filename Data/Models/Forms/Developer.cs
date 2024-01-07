using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;

namespace FormManager.Data.Models.Forms
{
    public class Developer : Form
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoGame> VideoGames { get; set; } = new List<VideoGame>();

        public DeveloperResponseData ToResponse()
        {
            DeveloperResponseData response = new DeveloperResponseData();
            SetHttpData(response);
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
    }
}
