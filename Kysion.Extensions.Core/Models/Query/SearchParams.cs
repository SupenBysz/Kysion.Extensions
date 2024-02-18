using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models.Query
{
    public class SearchParams
    {
        [JsonProperty("filter")]
        public List<Filter> Filter { get; set; } = new();

        [JsonProperty("orderBy")]
        public List<OrderBy> OrderBy { get; set; } = new();

        [JsonProperty("pageNum")]
        public int PageNum { get; set; } = 1;

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 20;

        [JsonProperty("isExport")]
        public bool IsExport { get; set; } = false;
    }
}
