using Microsoft.Win32;
using System.Diagnostics;

namespace Kysion.Extensions.Core.Helper
{
    /// <summary>
    /// 浏览器帮助类
    /// </summary>
    public class BrowserHelper
    {
        #region const

        /// <summary>
        /// 谷歌浏览器注册表地址
        /// </summary>
        private const string ChromeAppKey = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";
        /// <summary>
        /// 火狐浏览器注册表地址
        /// </summary>
        private const string FirefoxAppKey = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe";
        /// <summary>
        /// Edge浏览器注册表地址
        /// </summary>
        private const string MSEdgeAppKey = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe";
        /// <summary>
        /// 360极速浏览器注册表地址
        /// </summary>
        private const string Chrome360AppKey = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\360chrome.exe";

        #endregion

        #region private static methods

        /// <summary>
        /// 通过默认浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        private static void OpenDefaultBrowserUrl(string url)
        {
            Process.Start(url);
        }

        /// <summary>
        /// 通过IE浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        private static void OpenIeBrowserUrl(string url)
        {
            Process.Start("iexplore.exe", url);
        }

        /// <summary>
        /// 通过谷歌浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        private static void OpenGoogleBrowserUrl(string url)
        {
            try
            {
                // 通过注册表找到谷歌浏览器安装路径
                string chromeAppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + ChromeAppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + ChromeAppKey, "", null) ?? "");
                // 如果未找到谷歌浏览器则使用默认浏览器打开
                if (String.IsNullOrWhiteSpace(chromeAppFileName))
                {
                    OpenDefaultBrowserUrl(url);
                    return;
                }

                // 打开谷歌浏览器
                Process.Start(chromeAppFileName, url);
            }
            catch
            {
                // 如果发生异常则使用默认浏览器打开
                OpenDefaultBrowserUrl(url);
            }
        }

        /// <summary>
        /// 通过火狐浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        public static void OpenFirefoxBrowserUrl(string url)
        {
            try
            {
                // 通过注册表找到火狐浏览器安装路径
                string firefoxAppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + FirefoxAppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + FirefoxAppKey, "", null) ?? "");
                // 如果未找到火狐浏览器则使用默认浏览器打开
                if (String.IsNullOrWhiteSpace(firefoxAppFileName))
                {
                    OpenDefaultBrowserUrl(url);
                    return;
                }

                // 打开火狐浏览器
                Process.Start(firefoxAppFileName, url);
            }
            catch
            {
                // 如果发生异常则使用默认浏览器打开
                OpenDefaultBrowserUrl(url);
            }
        }

        /// <summary>
        /// 通过Edge浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        public static void OpenMSEdgeBrowserUrl(string url)
        {
            try
            {
                // 通过注册表找到Edge浏览器安装路径
                string msedgeAppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + MSEdgeAppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + MSEdgeAppKey, "", null) ?? "");
                // 如果未找到Edge浏览器则使用默认浏览器打开
                if (String.IsNullOrWhiteSpace(msedgeAppFileName))
                {
                    OpenDefaultBrowserUrl(url);
                    return;
                }

                // 打开Edge浏览器
                Process.Start(msedgeAppFileName, url);

                Process proc = new ();
                proc.StartInfo.FileName = url;
                proc.Start();
            }
            catch
            {
                // 如果发生异常则使用默认浏览器打开
                OpenDefaultBrowserUrl(url);
            }
        }
        /// <summary>
        /// 验证电脑上是否有需要用到的浏览器
        /// </summary>
        /// <returns></returns>
        public static string Verification()
        {
            // 通过注册表找到Edge浏览器安装路径
            string msedgeAppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + MSEdgeAppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + MSEdgeAppKey, "", null) ?? "");
            if (String.IsNullOrWhiteSpace(msedgeAppFileName))
            {
                // 通过注册表找到谷歌浏览器安装路径
                string chromeAppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + ChromeAppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + ChromeAppKey, "", null) ?? "");
                if (String.IsNullOrWhiteSpace(chromeAppFileName))
                {
                    // 通过注册表找到360极速浏览器安装路径
                    string chrome360AppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + Chrome360AppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + Chrome360AppKey, "", null) ?? "");
                    if (String.IsNullOrWhiteSpace(chrome360AppFileName))
                    {
                        return "";
                    }
                    return chrome360AppFileName;
                }
                return chromeAppFileName;
            }
            return msedgeAppFileName;
        }

        /// <summary>
        /// 通过360极速浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        public static void OpenChrome360BrowserUrl(string url)
        {
            try
            {
                // 通过注册表找到360极速浏览器安装路径
                string chrome360AppFileName = (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + Chrome360AppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + Chrome360AppKey, "", null) ?? "");
                // 如果未找到360极速浏览器则使用默认浏览器打开
                if (String.IsNullOrWhiteSpace(chrome360AppFileName))
                {
                    OpenDefaultBrowserUrl(url);
                    return;
                }

                // 打开360极速浏览器浏览器
                Process.Start(chrome360AppFileName, url);
            }
            catch
            {
                // 如果发生异常则使用默认浏览器打开
                OpenDefaultBrowserUrl(url);
            }
        }

        #endregion

        #region public static methods

        /// <summary>
        /// 通过浏览器打开Url
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="type">指定打开的浏览器类型</param>
        public static void OpenBrowserUrl(string url, BrowserType type = BrowserType.Default)
        {
            switch (type)
            {
                case BrowserType.Default:
                    OpenDefaultBrowserUrl(url);
                    break;
                case BrowserType.IE:
                    OpenIeBrowserUrl(url);
                    break;
                case BrowserType.Google:
                    OpenGoogleBrowserUrl(url);
                    break;
                case BrowserType.Firefox:
                    OpenFirefoxBrowserUrl(url);
                    break;
                case BrowserType.Edge:
                    OpenMSEdgeBrowserUrl(url);
                    break;
                case BrowserType.Chrome360:
                    OpenChrome360BrowserUrl(url);
                    break;
                default:
                    OpenDefaultBrowserUrl(url);
                    break;
            }
        }

        /// <summary>
        /// 指定浏览器地址打开Url
        /// </summary>
        /// <param name="fileName">指定的浏览器地址</param>
        /// <param name="url">Url地址</param>
        public static void OpenBrowserUrl(string fileName, string url)
        {
            Process.Start(fileName, url);
        }

        #endregion

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public enum BrowserType
        {
            /// <summary>
            /// 默认浏览器
            /// </summary>
            Default = 0,
            /// <summary>
            /// IE浏览器
            /// </summary>
            IE = 1,
            /// <summary>
            /// Google浏览器
            /// </summary>
            Google = 2,
            /// <summary>
            /// 火狐
            /// </summary>
            Firefox = 3,
            /// <summary>
            /// Microsoft Edge
            /// </summary>
            Edge = 4,
            /// <summary>
            /// 360极速浏览器
            /// </summary>
            Chrome360 = 5,
        }
    }
}
