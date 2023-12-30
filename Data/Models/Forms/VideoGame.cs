﻿using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;

namespace FormManager.Data.Models.Forms
{
    public class VideoGame : Form
    {
        public string Name { get; set; } = string.Empty;
        public VideoGameGenre Genre { get; set; } = VideoGameGenre.Misc;
        public DateTime ReleaseDate { get; set; }

        //TODO: Replace with Developer and Publisher - tables
        public string DeveloperName { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;

        private void SetResponseDataShort(VideoGameResponseDataShort response)
        {
            response.Name = Name;
            response.Genre = Genre;
            response.ReleaseDate = ReleaseDate;
            response.DeveloperName = DeveloperName;
            response.PublisherName = PublisherName;
        }

        public VideoGameResponseDataShort ToResponseShort()
        {
            VideoGameResponseDataShort response = new VideoGameResponseDataShort();
            SetResponseDataShort(response);
            SetHttpData(response);
            return response;
        }

        public VideoGameResponseData ToResponse()
        {
            VideoGameResponseData response = new VideoGameResponseData();
            SetResponseDataShort(response);
            SetHttpData(response);
            return response;
        }

        public static VideoGame FromRequestData(VideoGameRequestData requestData)
        {
            return new VideoGame {
                Id = requestData.Id,
                Name = requestData.Name
            };
        }
    }
}
