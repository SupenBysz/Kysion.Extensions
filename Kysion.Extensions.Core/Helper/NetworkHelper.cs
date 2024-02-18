using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace Kysion.Extensions.Core.Helper
{
    public class NetworkHelper
    {
        /// <summary>
        /// 判断UDP端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUseUDP(int port)
        {
            var inUse = false;
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipEndPoints = ipProperties.GetActiveUdpListeners();
            foreach (IPEndPoint item in ipEndPoints)
            {
                if (item.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        /// <summary>
        /// 判断TCP端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUseTCP(int port)
        {
            var inUse = false;
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipEndPoints = ipProperties.GetActiveTcpListeners();
            foreach (IPEndPoint item in ipEndPoints)
            {
                if (item.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        /// <summary>
        /// 通过基础端口获取一个本地未用的UDP端口
        /// </summary>
        /// <param name="basePort"></param>
        /// <returns></returns>
        public static int GetCanUseUdpPort(int basePort)
        {
            do
            {
                if (!PortInUseUDP(basePort))
                {
                    return basePort;
                }
                basePort++;
            } while (true);
        }

        /// <summary>
        /// 通过基础端口获取一个未本地使用的TCP端口
        /// </summary>
        /// <param name="basePort"></param>
        /// <returns></returns>
        public static int GetCanUseTcpPort(int basePort)
        {
            do
            {
                if (!PortInUseTCP(basePort))
                {
                    return basePort;
                }
                basePort++;
            } while (true);
        }

        /// <summary>
        /// 判断是否本机IPv4地址
        /// </summary>
        /// <param name="ipv4"></param>
        /// <returns></returns>
        public static bool IsLocalIpv4Address(string ipv4)
        {
            var checkIpAddress = Dns.GetHostAddresses(ipv4);
            var localIpAddress = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipa in localIpAddress)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    foreach (var item in checkIpAddress)
                    {
                        if (item.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (item.ToString() == ipa.ToString())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
