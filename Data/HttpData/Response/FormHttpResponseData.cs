namespace FormManager.Data.HttpData.Response
{
    /// <summary>
    /// Base class for form responses
    /// </summary>
    public class FormHttpResponseData : FormHttpData
    {
        public List<LogResponse> Log { get; set; } = new List<LogResponse>();
    }
}
