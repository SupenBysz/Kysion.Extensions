using Kysion.Extensions.Core.Models;

namespace Kysion.Extensions.Core.Singleton
{
    public class KysionConfig
    {
        #region SingletonInstance
        private KysionConfig(){}
        public static readonly KysionConfig Instance = new();
        #endregion

        public string HardwareUUID { get; set; } = string.Empty;

        public string DefaultLicenseType { get; set; } = string.Empty;

        public string BaseDomain { get; set; } = string.Empty;

        public string Accept { get; set; } = "application/json";

        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0";

        public int LogLength { get; set; } = 1000;

        public LicenseInfo LicenseInfo { get; set; } = new();

        public TokenInfo? TokenInfo
        {
            get => LicenseInfo.TokenInfo;
        }
    }
}
