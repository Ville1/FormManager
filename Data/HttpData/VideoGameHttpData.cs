namespace FormManager.Data.HttpData
{
    /// <summary>
    /// Base class for video game requests and responses
    /// </summary>
    public class VideoGameHttpData : FormHttpData
    {
        public string Name { get; set; } = string.Empty;
        public VideoGameGenre Genre { get; set; } = VideoGameGenre.Misc;
        public DateTime ReleaseDate { get; set; }
    }
}
