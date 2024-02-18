using Kysion.Extensions.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using System.Runtime.InteropServices;

namespace Kysion.Extensions.Web.ViewModels
{
#pragma warning disable CS0618 // 类型或成员已过时
    [ClassInterface(ClassInterfaceType.AutoDual)]
#pragma warning restore CS0618 // 类型或成员已过时
    [ComVisible(true)]
    public class WebInjectViewModel : AbstractWebInjectHost
    {
        private BaseViewModel<WebInjectViewModel> baseModel { get; set; }

        public WebInjectViewModel(
            CoreWebView2 coreWebView2,
            string injectScriptContent = "",
            string injectStyleContent = "",
            string injectName = "KysionWebHost",
            bool IsCustomAlert = false
            ) : base(
                coreWebView2: coreWebView2,
                injectScriptContent: injectScriptContent,
                injectStyleContent: injectStyleContent,
                injectName: injectName, IsCustomAlert)
        {
            baseModel = new BaseViewModel<WebInjectViewModel>("自动化脚本");
        }

        /// <summary>
        /// 成功回调函数
        /// </summary>
        /// <param name="args"></param>
        public override void OnSuccessMessage(string message, string? category = null)
        {
            baseModel.logger.LogInformation("Success Message：" + message);
            base.OnSuccessMessage(message, category);
        }

        /// <summary>
        /// 失败回调函数
        /// </summary>
        /// <param name="args"></param>
        public override void OnFailureMessage(string message, string? category = null)
        {
            baseModel.logger.LogError("Failure Message：" + message);
            base.OnFailureMessage(message, category);
        }

        /// <summary>
        /// 样式注入完成事件
        /// </summary>
        public override void OnInjectStyleComplete()
        {
            baseModel.logger.LogInformation("模型适配成功");
            base.OnInjectStyleComplete();
        }

        /// <summary>
        /// 脚本注入完成事件
        /// </summary>
        public override void OnInjectScriptComplete()
        {
            baseModel.logger.LogInformation("数据适配成功");
            base.OnInjectScriptComplete();
        }
    }
}
