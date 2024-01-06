namespace FormManager.Data.Models.Forms
{
    public class Publisher : Form
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoGame> VideoGames { get; set; } = new List<VideoGame>();
    }
}
