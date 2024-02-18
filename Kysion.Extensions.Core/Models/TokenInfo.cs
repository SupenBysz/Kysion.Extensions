using Kysion.Extensions.Core.Models.Base;
using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models
{
    public class TokenInfo : BaseModel
    {
        [JsonProperty("token")]
        public string Token
        {
            get => data.Token ??= string.Empty;
            set => SetAndNotify(() => data.Token = value);
        }


        [JsonProperty("expireAt")]
        public DateTime? ExpireAt
        {
            get => data.ExpireAt;
            set => SetAndNotify(()=> data.ExpireAt = value);
        }
    }
}
