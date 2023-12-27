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
    }
}
