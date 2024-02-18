using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models
{
    public class LogInfo
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; } = "";

        /// <summary>
        /// 日志内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; } = "";

        /// <summary>
        /// 日志创建时间
        /// </summary>
        [JsonProperty("create_at")]
        public string CreateAt { get; set; } = "";

        /// <summary>
        /// 日志级别
        /// </summary>
        [JsonProperty("level")]
        public LogLevel Level { get; set; } = LogLevel.Information;
    }
}
