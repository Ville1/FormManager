namespace FormManager.Data.HttpData.Response
{
    public class ValidationErrorResponse
    {
        /// <summary>
        /// Property name / list of errors
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
    }
}
