using System.IO;

namespace Kysion.Extensions.Core.Helper
{
    // 注意程序需要管理员权限
    public class HostsHelper
    {
        public static void SetHosts(string domain, string ip)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            var hosts = File.ReadAllLines(path);
            var list = hosts.ToList();
            var temp = hosts.ToList().FirstOrDefault(x => x.Contains(domain));
            if (string.IsNullOrEmpty(temp))
            {
                list.Add($"{ip} {domain}");
            }
            File.WriteAllLines(path, list.ToArray());
        }
        public static void RemoveHosts(string hosts_str)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            var hosts = File.ReadAllLines(path);
            var list = hosts.ToList();
            //int index = list.FindIndex(x => x.Contains(hosts_str));
            //list.RemoveAt(index);
            list.RemoveAll(x => x.Contains(hosts_str));
            //foreach (string item in list.ToArray())
            //{
            //    if (item.Contains(hosts_str))
            //    {
            //        list.Remove(item);
            //    }
            //}
            File.WriteAllLines(path, list.ToArray());
        }
    }
}
