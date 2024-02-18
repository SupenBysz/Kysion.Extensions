using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models.Query
{
    public class OrderBy
    {

        [JsonProperty("field")]
        public string Field { get; set; } = string.Empty;

        [JsonProperty("sort")]
        public string Sort { get; set; } = "asc";

    }
}
