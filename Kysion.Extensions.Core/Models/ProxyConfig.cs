using Kysion.Extensions.Core.Models.Base;
using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models
{
    public class ProxyConfig : BaseModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public long Id
        {
            get => data.Id ??= 0;
            set => SetAndNotify(() => data.Id = value);
        }

        /// <summary>
        /// 代理服务器IP
        /// </summary>
        [JsonProperty("proxyIp")]
        public string ProxyIp
        {
            get => data.ProxyIp ??= string.Empty;
            set => SetAndNotify(() => data.ProxyIp = value);
        }

        /// <summary>
        /// 代理服务器端口
        /// </summary>
        [JsonProperty("proxyPort")]
        public string ProxyPort
        {
            get => data.ProxyPort ??= string.Empty;
            set => SetAndNotify(() => data.ProxyPort = value);
        }

        /// <summary>
        /// 代理模式：0 http，1 https，2 socket
        /// </summary>
        [JsonProperty("proxyType")]
        public int ProxyType
        {
            get => data.ProxyType ??= 0;
            set => SetAndNotify(() => data.ProxyType = value);
        }

        /// <summary>
        /// 是否登陆：0 不用，1需要
        /// </summary>
        [JsonProperty("needLogin")]
        public int NeedLogin
        {
            get => data.NeedLogin ??= 0;
            set => SetAndNotify(() => data.NeedLogin = value);
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("username")]
        public string Username
        {
            get => data.Username ??= string.Empty;
            set => SetAndNotify(() => data.Username = value);
        }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("password")]
        public string Password
        {
            get => data.Password ??= string.Empty;
            set => SetAndNotify(() => data.Password = value);
        }

        /// <summary>
        /// 所属用户
        /// </summary>
        [JsonProperty("userId")]
        public long UserId
        {
            get => data.UserId ??= 0;
            set => SetAndNotify(() => data.UserId = value);
        }

        /// <summary>
        /// 最后使用消息：如登陆状态消息等
        /// </summary>
        [JsonProperty("lastUseMessage")]
        public string LastUseMessage
        {
            get => data.LastUseMessage ??= string.Empty;
            set => SetAndNotify(() => data.LastUseMessage = value);
        }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt
        {
            get => data.CreatedAt;
            set => SetAndNotify(() => data.CreatedAt = value);
        }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt
        {
            get => data.UpdatedAt;
            set => SetAndNotify(() => data.UpdatedAt = value);
        }
    }
}
