using Newtonsoft.Json;
using System.IO;

namespace Kysion.Extensions.Core.Services
{
    public class JsonFileService
    {
        #region 单例
        private static class SingletonInstance
        {
            public static JsonFileService INSTANCE = new();
        }

        public static JsonFileService Instance
        {
            get => SingletonInstance.INSTANCE;
        }
        private JsonFileService(){}
        #endregion

        public static void SaveToJson(string fileName, object obj)
        {
            lock (SingletonInstance.INSTANCE)
            {
                // 创建一个 StreamWriter 对象来写入 JSON 数据
                using (var writer = new StreamWriter(fileName))
                {
                    // 使用 JsonSerializer 来将对象序列化为 JSON 字符串
                    var serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore, // this matters when serializing Id.
                        DefaultValueHandling = DefaultValueHandling.Include
                    };
                    serializer.Serialize(writer, obj);
                }
            }
        }

        public static T LoadFromJson<T>(string fileName) where T : new()
        {
            // 检查文件是否存在
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("文件未找到");
            }

            lock (SingletonInstance.INSTANCE)
            {
                // 创建一个 StreamReader 对象来读取 JSON 数据
                using (var reader = new StreamReader(fileName))
                {
                    // 使用 JsonSerializer 来将 JSON 字符串反序列化为对象
                    var serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore, // this matters when serializing Id.
                        DefaultValueHandling = DefaultValueHandling.Include
                    };
                    return (T?)serializer.Deserialize(reader, typeof(T)) ?? new();
                }
            }
        }

        //public static Task SaveConfig(IList<WebGroupInfoJson> WebGroupItems, string fileName = "data.json")
        //{
        //    return Task.Run(() =>
        //    {
        //        var basePath = Directory.GetCurrentDirectory().Replace("\\", "/") + Constant.DefaultConfFolderPath;

        //        try
        //        {
        //            if (!Directory.Exists(basePath))
        //            {
        //                if (!Funs.CreateDirectory(basePath))
        //                {
        //                    App.GetService<ISnackbarService>()?.ShowAsync("操作提示", "数据文件保存文件夹创建失败", SymbolRegular.Info28, ControlAppearance.Danger);
        //                    return;
        //                }
        //            }

        //            SaveToJson(basePath + fileName, WebGroupItems);
        //        }
        //        catch (Exception)
        //        {
        //            App.GetService<ISnackbarService>()?.ShowAsync("操作提示", "数据文件保存失败", SymbolRegular.Info28, ControlAppearance.Danger);
        //        }
        //    });
        //}
    }
}
