using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Kysion.Extensions.Core.Utils
{
    public static class Funs
    {
        /// <summary>
        /// 延时指定时间后执行函数
        /// </summary>
        /// <param name="func"></param>
        /// <param name="ms"></param>
        public static void SetTimeout(Action func, int ms)
        {
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(ms);
                func.Invoke();
            });
        }

        /// <summary>
        /// 周期性执行指定函数
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="ms"></param>
        public static async void SetInterval(Func<Task<bool>> fun, int ms)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    if (await fun())
                    {
                        await Task.Delay(ms);
                        SetInterval(fun, ms);
                    }
                }
                catch (Exception)
                {
                    //
                }
            });
        }

        /// <summary>
        /// Url 前缀检测，不含http:// 或 https:// 时强制加 https://
        /// </summary>
        /// <param name="url"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static Uri MakeUri(string url, string schema = "https://")
        {
            if(url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("ws://") || url.StartsWith("wss://"))
            {
                return new Uri(url);
            }
            else
            {
                return new Uri(schema + url);
            }
        }

        /// <summary>
        /// 深度复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T? DeepCopy<T>(T source)
        {
            if (source == null)
                return default;

            // 创建一个新的对象，用于存储深拷贝的结果
            T destination = Activator.CreateInstance<T>();

            // 获取源对象的类型
            Type sourceType = source.GetType();

            // 遍历源对象的所有字段
            foreach (FieldInfo fieldInfo in sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                // 设置字段可访问
                fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
            }

            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T? FindVisualParent<T>(Visual child) where T : Visual
        {
            var parent = VisualTreeHelper.GetParent(child);
            while (parent != null && parent.GetType() != typeof(T))
                parent = VisualTreeHelper.GetParent(parent);

            return parent as T;
        }

        /// <summary>
        /// 递归创建目录
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string directoryPath)
        {
            try
            {
                string[] pathParts = directoryPath.Replace("/","\\").Split("\\");
                for (var i = 0; i < pathParts.Length; i++)
                    {
                        // Correct part for drive letters
                        if (i == 0 && pathParts[i].Contains(":"))
                        {
                            pathParts[i] = pathParts[i] + "\\";
                        } // Do not try to create last part if it has a period (is probably the file name)
                        else if (i == pathParts.Length - 1 && pathParts[i].Contains("."))
                        {
                            return true;
                        }
                        if (i > 0)
                        {
                            pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);
                        }
                        if (!Directory.Exists(pathParts[i]))
                        {
                            Directory.CreateDirectory(pathParts[i]);
                        }
                    }
                    return true;
            }
            catch (Exception ex)
            {
                var message = "创建 " + directoryPath + " 目录失败：" + ex.Message;
                Console.WriteLine(message);
                return false;
            }
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomAccount()
        {
            // 创建一个 StringBuilder 对象用于存储生成的随机字符串
            StringBuilder sb = new StringBuilder();

            // 定义一个包含大小写字母的字符串用于生成随机字符
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            // 使用 RandomNumberGenerator 类生成一个随机数生成器
            Random rng = new Random();

            // 生成 32 个随机字符，并将其添加到 StringBuilder 中
            for (int i = 0; i < 8; i++)
            {
                int index = rng.Next(alphabet.Length);
                char randomChar = alphabet[index];
                sb.Append(randomChar);
            }

            // 移除可能生成的额外的前导空格

            // 返回生成的随机登录账号
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 生成指定范围随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GenerateRandomNumber(int min, int max)
        {
            // 创建一个 Random 对象
            Random random = new Random();

            // 使用 Next 方法生成指定范围内的随机整数
            int randomNumber = random.Next(min, max + 1);

            return randomNumber;
        }
    }
}
