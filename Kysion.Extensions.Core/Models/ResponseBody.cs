using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models
{
    public class ResponseBody<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; } = 0;
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsSuccess { get => Code == 0; }
        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("time")]
        public DateTime DateTime { get; set; }
    }

    public class ResultQuery<T>
    {
        [JsonProperty("records")]
        public T[] Records { get; set; } = {};

        [JsonProperty("pageNum")]
        public int PageNum { get; set; } = 1;

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 20;

        [JsonProperty("pageTotal")]
        public int PageTotal { get; set; } = 0;

        [JsonProperty("total")]
        public int Total {  get; set; } = 0;
    }
}
