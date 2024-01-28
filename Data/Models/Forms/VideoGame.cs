using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;
using FormManager.Services;

namespace FormManager.Data.Models.Forms
{
    public class VideoGame : Form
    {
        public string Name { get; set; } = string.Empty;
        public VideoGameGenre Genre { get; set; } = VideoGameGenre.Misc;
        public DateTime ReleaseDate { get; set; }
        public Guid? DeveloperId { get; set; } = null;
        public Developer? Developer { get; set; } = null;
        public Guid? PublisherId { get; set; } = null;
        public Publisher? Publisher { get; set; } = null;

        private void SetResponseDataShort(VideoGameResponseDataShort response)
        {
            response.Name = Name;
            response.Genre = Genre;
            response.ReleaseDate = ReleaseDate;
            response.DeveloperName = Developer?.Name ?? string.Empty;
            response.PublisherName = Publisher?.Name ?? string.Empty;
        }

        public VideoGameResponseDataShort ToResponseShort(DatabaseService database)
        {
            VideoGameResponseDataShort response = new VideoGameResponseDataShort();
            SetResponseDataShort(response);
            SetHttpData(response, database);
            return response;
        }

        public VideoGameResponseData ToResponse(DatabaseService database)
        {
            VideoGameResponseData response = new VideoGameResponseData();
            SetResponseDataShort(response);
            SetHttpData(response, database);
            SetHttpResponseLogData(response, database);

            response.DeveloperId = DeveloperId;
            response.PublisherId = PublisherId;

            return response;
        }

        public static VideoGame FromRequestData(VideoGameRequestData requestData)
        {
            return new VideoGame {
                Id = requestData.Id,
                Name = requestData.Name,
                DeveloperId = requestData.DeveloperId,
                PublisherId = requestData.PublisherId
            };
        }
    }
}
