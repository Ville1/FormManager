using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;

namespace FormManager.Data.Models.Forms
{
    public class Publisher : Form
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoGame> VideoGames { get; set; } = new List<VideoGame>();

        public PublisherResponseData ToResponse()
        {
            PublisherResponseData response = new PublisherResponseData();
            SetHttpData(response);
            response.Name = Name;
            return response;
        }

        public static Publisher FromRequestData(PublisherRequestData requestData)
        {
            return new Publisher {
                Id = requestData.Id,
                Name = requestData.Name
            };
        }
    }
}
