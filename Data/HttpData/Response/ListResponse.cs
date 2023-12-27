using System.Text.Json.Serialization;

namespace FormManager.Data.HttpData.Response
{
    public class ListResponse<DataT>
    {
        public ListResponse(IEnumerable<DataT> data, long totalRows, long pageSize)
        {
            Rows = data.ToList();
            TotalRows = totalRows;
            TotalPages = totalRows == 0 ? 0 : (long)Math.Ceiling(totalRows / (double)pageSize);
        }

        public List<DataT> Rows { get; set; } = new List<DataT>();

        public long TotalRows { get; set; }

        public long TotalPages { get; set; }
    }
}
