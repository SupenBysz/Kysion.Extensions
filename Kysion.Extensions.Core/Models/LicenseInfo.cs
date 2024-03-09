using Kysion.Extensions.Core.Singleton;
using Kysion.Extensions.Core.Helper;
using Kysion.Extensions.Core.Models.Base;
using Newtonsoft.Json;

namespace Kysion.Extensions.Core.Models
{
    public class LicenseInfo : BaseModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public string Id
        {
            get => data.Id ?? string.Empty;
            set => data.Id = value;
        }
        /// <summary>
        /// 终端标识符
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier
        {
            get => data.Identifier ?? string.Empty;
            set => data.Identifier = value;
        }

        [JsonProperty("licenseExpires")]
        public DateTime? Expires
        {
            get => data.Expires;
            set => data.Expires = value;
        }

        public string LicenseStatus
        {
            get
            {
                if(LicenseCode == string.Empty || Expires == null)
                    return "未授权";

                if (!Validation)
                    return "无效授权";

                if (TokenInfo == null)
                    return "未激活";

                if (Expires != null)
                {
                    TimeSpan timeSpan = Expires.Value! - DateTime.Now;
                    if(timeSpan < TimeSpan.Zero)
                        return "授权已过期限";
                }

                return data.Expires.ToString("yyyy-MM-dd");
            }
        }
        
        /// <summary>
        /// 应用标题
        /// </summary>
        [JsonProperty("licenseTitle")]
        public string Title
        {
            get => data.Title ?? string.Empty;
            set => data.Title = value;
        }
        /// <summary>
        /// 应用标题
        /// </summary>
        [JsonProperty("registrationAt")]
        public DateTime? RegistrationAt
        {
            get => data.RegistrationAt;
            set => data.RegistrationAt = value;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty("userId")]
        public string UserId
        {
            get => data.UserId ?? string.Empty;
            set => data.UserId = value;
        }


        /// <summary>
        /// 终端许可证配置
        /// </summary>
        [JsonProperty("licenseCode")]
        public string LicenseCode
        {
            get => data.LicenseCode ?? string.Empty;
            set => data.LicenseCode = value;
        }

        /// <summary>
        /// 终端许可证配置
        /// </summary>
        [JsonProperty("licenseType")]
        public string LicenseType
        {
            get => data.LicenseType ?? string.Empty;
            set => data.LicenseType = value;
        }

        /// <summary>
        /// 应用配置更新时间
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt
        {
            get => data.UpdatedAt;
            set => data.UpdatedAt = value;
        }

        /// <summary>
        /// 应用配置更新时间
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt
        {
            get => data.CreatedAt;
            set => data.CreatedAt = value;
        }

        [JsonProperty("token")]
        public TokenInfo? TokenInfo
        {
            get => data.TokenInfo;
            set => data.TokenInfo = value;
        }

        public bool Validation 
        {
            get
            {
                if (KysionConfig.Instance.HardwareUUID.Length <= 0)
                {
                    return false;
                }

                var checkCode = KysionConfig.Instance.HardwareUUID.Substring(1) +
                    Title +
                    KysionConfig.Instance.DefaultLicenseType +
                    (RegistrationAt?.ToString("yyyy-MM-dd hh:mm:ss") ?? string.Empty) +
                    (Expires?.ToString("yyyy-MM-dd hh:mm:ss") ?? string.Empty) +
                    (UserId + "UserId").Substring(1) +
                    "Kysion";
                var lcode = EncryptHelper.MD5(checkCode);
                return lcode == LicenseCode;
                //return true;
            }
        }
    }
}
