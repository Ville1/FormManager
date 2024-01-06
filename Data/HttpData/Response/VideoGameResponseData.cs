namespace FormManager.Data.HttpData.Response
{
    public class VideoGameResponseData : VideoGameResponseDataShort
    {
        public Guid? DeveloperId { get; set; }
        public Guid? PublisherId { get; set; }
    }
}
