namespace FormManager.Data.HttpData.Response
{
    public class ListResponse<DataT>
    {
        public List<DataT> Rows { get; set; } = new List<DataT>();
        public long TotalRows { get; set; }
    }
}
