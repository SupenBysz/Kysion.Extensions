using Kysion.Extensions.Core.Singleton;
using Kysion.Extensions.Core.Services;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Kysion.Extensions.Core.Models;

namespace Kysion.Extensions.Core.Helper
{
    public class HardwareHelper
    {
        internal static string GetLocalMacAddress()
        {
            var result = string.Empty;
            try
            {
                // 获取本地计算机的所有网络接口
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                if (networkInterfaces != null)
                {
                    // 遍历每个网络接口
                    foreach (NetworkInterface networkInterface in networkInterfaces)
                    {
                        if (networkInterface.OperationalStatus == OperationalStatus.Up)
                        {
                            // 获取网络接口的 MAC 地址
                            PhysicalAddress macAddress = networkInterface.GetPhysicalAddress();
                            if (macAddress != null)
                            {
                                var macAddressStr = macAddress.ToString();
                                if (result.Length > 0 && macAddressStr.Length > 0)
                                {
                                    result += ",";
                                }
                                result += macAddressStr;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //
            }

            return result;
        }

        public static Task<LicenseInfo?> MakeHardwareUUID()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var key = GetDiskSerialNumber() + ";" + GetCpuSerialNumber() + ";" + GetHardDiskID() + ";" + KysionConfig.Instance.DefaultLicenseType;
                    //var key = GetLocalMacAddress() + ";" + GetDiskSerialNumber() + ";" + GetCpuSerialNumber() + ";" + GetHardDiskID() + ";" + KysionConfig.Instance.DefaultLicenseType;
                    // 此处获取分区序列码会导致page加载失败，原因未知
                    //key += ";" + GetdiskID();
                    string reesult = EncryptHelper.MD5(key)[8..];
                    KysionConfig.Instance.HardwareUUID = reesult;
                    return await HttpService.DeviceAPI.GetDeviceRegister();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        internal static string GetDiskSerialNumber()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return string.Empty;

            try
            {
                var diskSerial = string.Empty;
                ManagementClass cimobject = new("Win32_DiskDrive");
                ManagementObjectCollection moc = cimobject.GetInstances();
                foreach (ManagementObject mo in moc.Cast<ManagementObject>())
                {
                    diskSerial += (string)mo.Properties["Model"].Value;
                }
                return diskSerial;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        internal static string GetHardDiskID()
        {
            string strHardDiskID = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new("SELECT * FROM Win32_PhysicalMedia");

                foreach (ManagementObject mo in searcher.Get().Cast<ManagementObject>())
                {
                    var did = mo["SerialNumber"]?.ToString()?.Trim() ?? string.Empty;

                    if (strHardDiskID.Length > 0 && did.Length > 0)
                    {
                        strHardDiskID += ",";
                    }

                    strHardDiskID  += did;
                }
                return strHardDiskID;
            }
            catch
            {
                return strHardDiskID;
            }
        }

        internal static string GetCpuSerialNumber()
        {
            var cpuSerial = string.Empty;
            ManagementClass mcCpu = new("win32_Processor");
            ManagementObjectCollection mocCpu = mcCpu.GetInstances();
            foreach (ManagementObject m in mocCpu.Cast<ManagementObject>())
            {
                var cpuId = m["ProcessorId"].ToString() ?? string.Empty;

                if(cpuSerial.Length > 0 && cpuId.Length > 0)
                {
                    cpuSerial += ",";
                }
                cpuSerial += cpuId;
            }
            return cpuSerial;
        }


        /// <summary>
		/// GetVolumeInformation
		/// </summary>
		/// <param name="lpRootPathName">欲获取信息的那个卷的根路径</param>
		/// <param name="lpVolumeNameBuffer">用于装载卷名（卷标）的一个字串 </param>
		/// <param name="nVolumeNameSize">lpVolumeNameBuffer字串的长度</param>
		/// <param name="lpVolumeSerialNumber">用于装载磁盘卷序列号的变量</param>
		/// <param name="lpMaximumComponentLength">指定一个变量，用于装载文件名每一部分的长度。例如，在“c:\component1\component2.ext”的情况下，它就代表component1或component2名称的长度 .</param>
		/// <param name="lpFileSystemFlags">用于装载一个或多个二进制位标志的变量。对这些标志位的解释如下：
		/// FS_CASE_IS_PRESERVED 文件名的大小写记录于文件系统
		/// FS_CASE_SENSITIVE 文件名要区分大小写
		/// FS_UNICODE_STORED_ON_DISK 文件名保存为Unicode格式
		/// FS_PERSISTANT_ACLS 文件系统支持文件的访问控制列表（ACL）安全机制
		/// FS_FILE_COMPRESSION 文件系统支持逐文件的进行文件压缩
		/// FS_VOL_IS_COMPRESSED 整个磁盘卷都是压缩的
		///</param>
		/// <param name="lpFileSystemNameBuffer">指定一个缓冲区,用于装载文件系统的名称（如FAT，NTFS以及其他）       </param>
		/// <param name="nFileSystemNameSize">lpFileSystemNameBuffer字串的长度</param>
		/// <returns></returns>
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool GetVolumeInformation(string lpRootPathName, string lpVolumeNameBuffer, int nVolumeNameSize, ref int lpVolumeSerialNumber, int lpMaximumComponentLength, int lpFileSystemFlags, string lpFileSystemNameBuffer, int nFileSystemNameSize);
        /// <summary>
        /// 获取硬盘ID
        /// </summary>
        /// <returns></returns>
        internal static string GetdiskID(string driver = "")
        {
            // 在 Windows 上，通过获取环境变量 %SystemDrive% 来确定系统所在的分区
            try
            {
                if (driver == "")
                {
                    driver = Environment.GetEnvironmentVariable("SystemDrive") ?? string.Empty;
                }

                const int MAX_FILENAME_LEN = 256;
                int retVal = 0;
                int a = 0;
                int b = 0;
                string str1 = string.Empty;
                string str2 = string.Empty;

                if (!driver.EndsWith(":") && !driver.EndsWith(@"\"))
                {
                    driver += @":";
                }
                if (driver.EndsWith(":") && !driver.EndsWith(@"\"))
                {
                    driver += @"\";
                }

                GetVolumeInformation(
                    driver,
                    str1,
                    MAX_FILENAME_LEN,
                    ref retVal,
                    a,
                    b,
                    str2,
                    MAX_FILENAME_LEN);

                return Convert.ToString(retVal, 16).ToUpper();
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
    }
}