namespace FormManager.Data.HttpData.Request
{
    public class VideoGameRequestData : VideoGameHttpData
    {
        public Guid? DeveloperId { get; set; }
        public Guid? PublisherId { get; set; }
    }
}
