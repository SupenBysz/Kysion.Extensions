using Kysion.Extensions.Core.Utils;
using Kysion.Extensions.Core.ViewModels;
using Kysion.Extensions.Web.Contracts;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.IO;
using System.Net;
using System.Windows;

namespace Kysion.Extensions.Web.ViewModels
{
    public abstract class BaseWebViewModel<T> : BaseViewModel<T>, IBaseWebViewModel, IBaseWebInjectHost
    {
        protected BaseWebViewModel(string title, Uri webAddr, string? dataFolder = null) : base(title)
        {
            BaseDomain = webAddr;
            if (webBrowser != null) return;

            DataFolder = dataFolder != null && dataFolder.Length >0 ? dataFolder : GetDataFolder();
            Funs.CreateDirectory(DataFolder);

            webBrowser = new WebView2 {
                CreationProperties = new CoreWebView2CreationProperties
                {
                    UserDataFolder = DataFolder,
                },
            };

            browser.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
            browser.Source = BaseDomain;
        }

        public Uri BaseDomain { get; private set; }
        /// <summary>
        /// WebView2 组件
        /// </summary>
        private WebView2? webBrowser { get; set; }
        /// <summary>
        /// 给子类调用的 WebView2 组件
        /// </summary>
        public WebView2 browser { get => webBrowser!; }
        /// <summary>
        /// WebView2 数据文件夹
        /// </summary>
        public string DataFolder { get; protected set; } = string.Empty;
        /// <summary>
        /// 样式注入完成事件
        /// </summary>
        public event Action? InjectStyleCompleteEvent;
        /// <summary>
        /// 脚本注入完成事件
        /// </summary>
        public event Action? InjectScriptCompleteEvent;
        /// <summary>
        /// 注入模型
        /// </summary>
        public AbstractWebInjectHost? InjectViewModel { get; set; }
        /// <summary>
        /// Cookie
        /// </summary>
        public List<Cookie> CookieArr { get; set; } = new();

        public virtual void OnBack(object sender, RoutedEventArgs e)
        {
            if (browser.IsInitialized == true && browser!.CanGoBack)
                browser.GoBack();
        }

        public virtual void OnHome(object sender, RoutedEventArgs e)
        {
            if (browser.IsInitialized == true)
                browser.Source = BaseDomain;
        }

        public virtual void OnRefresh(object sender, RoutedEventArgs e)
        {
            if (browser.IsInitialized == true)
                browser.Reload();
        }

        public virtual string GetDataFolder()
        {
            return Directory.GetCurrentDirectory() + "/data/web/userdata";
        }

        protected virtual void OnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            InjectViewModel = InjectHandler(browser.CoreWebView2);
            if (InjectViewModel != null)
            {
                InjectViewModel.SuccessMessageEvent += OnSuccessMessage;
                InjectViewModel.FailureMessageEvent += OnFailureMessage;
                InjectViewModel.InjectStyleCompleteEvent += () =>
                {
                    OnInjectStyleComplete();
                    InjectStyleCompleteEvent?.Invoke();
                };
                InjectViewModel.InjectScriptCompleteEvent += () =>
                {
                    OnInjectScriptComplete();
                    InjectScriptCompleteEvent?.Invoke();
                };
            }

            browser.ZoomFactor = 1;
            browser.CoreWebView2.SourceChanged += OnWebView2SourceChanged;
            browser.CoreWebView2.NewWindowRequested += OnWebView2NewWindowRequested;
            browser.CoreWebView2.DOMContentLoaded += OnWebView2DOMContentLoaded; ;
            browser.CoreWebView2.WebResourceRequested += OnWebView2WebResourceRequested;
            browser.CoreWebView2.WebResourceResponseReceived += OnWebView2WebResourceResponseReceived;
            browser.CoreWebView2.ContextMenuRequested += OnWebView2ContextMenuRequested;
            browser.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
            browser.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            browser.CoreWebView2.Settings.AreDevToolsEnabled = true;
            browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            browser.CoreWebView2.Settings.AreHostObjectsAllowed = true;
            browser.CoreWebView2.Settings.IsWebMessageEnabled = true;

            foreach (var cookie in CookieArr)
            {
                var webviewCookie = browser.CoreWebView2.CookieManager.CreateCookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path);
                browser.CoreWebView2.CookieManager.AddOrUpdateCookie(webviewCookie);
            }

            var filters = MakeResourceRequestedFilter();
            foreach (var item in filters)
                browser.CoreWebView2.AddWebResourceRequestedFilter(item.Key, item.Value);
        }

        protected virtual AbstractWebInjectHost InjectHandler(CoreWebView2 coreWebView2)
        {
            return new WebInjectViewModel(coreWebView2);
        }

        protected virtual List<KeyValuePair<string, CoreWebView2WebResourceContext>> MakeResourceRequestedFilter()
        {
            return new()
            {
                KeyValuePair.Create( "*", CoreWebView2WebResourceContext.Fetch ),
                KeyValuePair.Create( "*", CoreWebView2WebResourceContext.XmlHttpRequest ),
            };
        }

        protected virtual void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e) { }

        protected virtual void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e) { }

        protected virtual void OnWebView2ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e) { }

        protected virtual void OnWebView2DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e) { }

        protected virtual void OnWebView2NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e) { }

        protected virtual void OnWebView2SourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e) { }

        protected virtual void OnWebView2WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e) { }

        protected virtual void OnWebView2WebResourceResponseReceived(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e) { }

        public virtual void OnInjectStyleComplete() { }

        public virtual void OnInjectScriptComplete() { }

        public virtual void OnSuccessMessage(string message, string? category = null) { }

        public virtual void OnFailureMessage(string message, string? category = null) { }
    }
}
