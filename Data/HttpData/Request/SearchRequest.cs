using System.Reflection;
using System.Text.Json.Serialization;
using FormManager.Utils;

namespace FormManager.Data.HttpData.Request
{
    public class SearchRequest
    {
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; } = 0;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 100;

        [JsonPropertyName("filters")]
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();

        public void Validate()
        {
            CurrentPage = Math.Max(0, CurrentPage);
            PageSize = Math.Max(1, PageSize);
        }

        public FilterContainer ParseFilters(List<Filter> filterConfig)
        {
            FilterContainer filters = new FilterContainer();
            foreach(Filter filter in filterConfig) {
                filters.Filters.Add(new Filter(filter, Filters.ContainsKey(filter.RequestPropertyName) ? Filters[filter.RequestPropertyName] : string.Empty));
            }
            return filters;
        }

        public List<TResult> Paginate<TResult>(IEnumerable<TResult> results)
        {
            return results.Skip(CurrentPage * PageSize).Take(PageSize).ToList();
        }
    }

    public class Filter
    {
        public FilterType Type { get; set; } = FilterType.Text;
        public string RequestPropertyName { get; set; } = string.Empty;
        public string ObjectPropertyName { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;

        public Filter(FilterType type, string requestPropertyName, string objectPropertyName)
        {
            Type = type;
            RequestPropertyName = requestPropertyName;
            ObjectPropertyName = objectPropertyName;
        }

        public Filter(FilterType type, string requestPropertyName)
        {
            Type = type;
            RequestPropertyName = requestPropertyName;
            ObjectPropertyName = requestPropertyName.Capitalize();
        }

        public Filter(Filter filter, string value)
        {
            Type = filter.Type;
            RequestPropertyName = filter.RequestPropertyName;
            ObjectPropertyName = filter.ObjectPropertyName;
            FilterValue = value;
        }

        public bool IsMatch(object o)
        {
            switch (Type) {
                case FilterType.Text:
                    PropertyInfo? property = o.GetType().GetProperties().FirstOrDefault(p => p.Name == ObjectPropertyName);
                    if(property == null) {
                        throw new Exception(string.Format("Type \"{0}\" has no property \"{1}\"", o.GetType().FullName, ObjectPropertyName));
                    }
                    object? objectValue = property.GetValue(o);
                    string? valueNullable = objectValue == null ? string.Empty : objectValue.ToString();
                    string value = valueNullable ?? string.Empty;
                    return value.ToLower().Contains(FilterValue.ToLower());
                default:
                    throw new NotImplementedException(string.Format("Filter type not implemented \"{0}\"", Type));
            }
        }
    }

    public class FilterContainer
    {
        public List<Filter> Filters { get; set; } = new List<Filter>();

        public bool IsMatch(object o)
        {
            foreach(Filter filter in Filters) {
                if (!filter.IsMatch(o)) {
                    return false;
                }
            }
            return true;
        }
    }
}
