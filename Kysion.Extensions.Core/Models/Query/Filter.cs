using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models.Query
{
    public class Filter
    {
        [JsonProperty("field")]
        public string Field { get; set; } = string.Empty;

        [JsonProperty("where")]
        public string Where { get; set; } = "=";

        [JsonProperty("isOrWhere")]
        public bool IsOrWhere { get; set; } = false;

        [JsonProperty("value")]
        public object Value { get; set; } = string.Empty;

        [JsonProperty("isNullValue")]
        public bool IsNullValue { get; set; } = false;

        [JsonProperty("modifier")]
        public string Modifier { get; set; } = string.Empty;
    }
}
