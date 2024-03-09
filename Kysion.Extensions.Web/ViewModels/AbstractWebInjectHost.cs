using Kysion.Extensions.Web.Contracts;
using Microsoft.Web.WebView2.Core;

namespace Kysion.Extensions.Web.ViewModels
{
    public abstract class AbstractWebInjectHost : IBaseWebInjectHost
    {
        public CoreWebView2 CoreWebView2 { get; set; }
        /// <summary>
        /// 脚本执行成功事件
        /// </summary>
        public event Action<string, string?>? SuccessMessageEvent;
        /// <summary>
        /// 脚本执行失败时间
        /// </summary>
        public event Action<string, string?>? FailureMessageEvent;
        /// <summary>
        /// 样式注入完成事件
        /// </summary>
        public event Action? InjectStyleCompleteEvent;
        /// <summary>
        /// 脚本注入完成事件
        /// </summary>
        public event Action? InjectScriptCompleteEvent;

        public string InjectName { get; set; } = string.Empty;

        public bool IsCustomAlert { get; set; } = false;

        public string InjectScriptContent { get; set; } = string.Empty;

        public string InjectStyleContent { get; set; } = string.Empty;

        public AbstractWebInjectHost(
            CoreWebView2 coreWebView2,
            string injectScriptContent = "",
            string injectStyleContent = "",
            string injectName = "KysionWebHost",
            bool isCustomAlert = false
            )
        {
            CoreWebView2 = coreWebView2;
            InjectScriptContent = injectScriptContent;
            InjectStyleContent = injectStyleContent;
            InjectName = injectName;
            IsCustomAlert = isCustomAlert;

            CoreWebView2.AddHostObjectToScript(InjectName, this);
            CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(InjectScript);
        }

        /// <summary>
        /// 成功回调函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public virtual void OnSuccessMessage(string message, string? category = null)
        {
            SuccessMessageEvent?.Invoke(message, category);
        }

        /// <summary>
        /// 失败回调函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public virtual void OnFailureMessage(string message, string? category = null)
        {
            FailureMessageEvent?.Invoke(message, category);
        }

        /// <summary>
        /// 样式注入完成事件
        /// </summary>
        public virtual void OnInjectStyleComplete()
        {
            InjectStyleCompleteEvent?.Invoke();
        }

        /// <summary>
        /// 脚本注入完成事件
        /// </summary>
        public virtual void OnInjectScriptComplete()
        {
            InjectScriptCompleteEvent?.Invoke();
        }

        public string InjectScript
        {
            get
            {
                var result = string.Empty;

                result += baseScriptFunc;
                result += makeStateFunc;
                result += InjectScriptContent;

                if (InjectScriptContent.Contains("$InjectStyleContent$"))
                    result = result.Replace("$InjectStyleContent$", InjectStyleContent);
                else
                    result += InjectStyle;

                if (IsCustomAlert)
                    result += customAlert;

                result = result.Replace("ExecSuccessMessage(", "ExecSuccessMessage_" + InjectName + "(");
                result = result.Replace("ExecFailureMessage(", "ExecFailureMessage_" + InjectName + "(");
                return result;
            }
        }

        public string InjectStyle
        {
            get
            {
                var styleContent = string.Empty;

                if (InjectStyleContent != string.Empty)
                    styleContent = @"createStyleByHead(document, `"+ InjectStyleContent + @"`);";

                return @"
function initStyle() {
    setTimeout(()=>{
        if(document.head == null)
            return initStyle();

        "+ styleContent + @"
        getKysionWebHost().OnInjectStyleComplete();
    }, 200);
}
initStyle();
";
            }
        }

        protected virtual string baseScriptFunc
        {
            get => @"
function getUrlParam(key) {
    var reg = new RegExp('(^|&)' + key + '=([^&]*)(&|$)'); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) {
        return decodeURIComponent(r[2]);
    }
    return null; //返回参数值
}

function setInputValue(dom, value) {
    let setValue = Object.getOwnPropertyDescriptor(dom.__proto__, 'value').set
    setValue.call(dom, value)
    dom.dispatchEvent(new Event('input', {bubbles: true}));
}

function formatParams(params = {}) {
    const p = new URLSearchParams();
    for (const key in params) {
        p.append(key, params[key]);
    }
    return p;
}

function requestSend(url, method, body = {}) {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();

        const params = formatParams(body);

        if (method == 'GET') {
            if(url.indexOf('?') > 0)
                url += '&' + params.toString();
            else
                url += '?' + params.toString();
        } else if (method == 'POST') {
            xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xhr.setRequestHeader('Accept', 'application/json');
        }
        
        xhr.open(method, url, true);
        xhr.onload = function (e) {
            if (xhr.readyState === 4 && xhr.status === 200) {
                return resolve(xhr.responseText);
            }
            else {
                return resolve('');
            }
        };

        xhr.onerror = function (e) {
            return reject(e);
        };

        // xhr will ignore body when the request is GET or HEAD
        if(method == 'GET')
            xhr.send();
        else
            xhr.send(params.toString());
    });
}

function querySelector(code, param = {winObj: window, filterFunc: null}) {
	winObj = param.winObj ??= window;
	filterFunc = param.filterFunc;

	let ret = winObj.document.querySelector(code);
	if(ret) {
		if(filterFunc != null)
			filterFunc(ret)

		return ret;
	}

	let frameArr = winObj.document.body.childNodes;
	for (let index = 0; index < frameArr.length; index++) {
		let frameObj = frameArr[index];

		if(frameObj.contentWindow) {
			ret = querySelector(code, {winObj: frameObj.contentWindow, filterFunc: filterFunc});
			if(ret) {
				return ret;
			}
		}
	}
	return ret;
}

function querySelectorAll(code, param = {winObj: window, filterFunc: null}) {
	winObj = param.winObj ??= window;
	filterFunc = param.filterFunc;

	let items = [];
	let queryArr = winObj.document.querySelectorAll(code);
	if(queryArr.length > 0) {
		if(filterFunc != null) {
			queryArr = filterFunc(queryArr);
		}
		
		if(queryArr.length > 0){
			queryArr.forEach(element => {
				items.push(element);
			});
		}
	}

	let frameArr = winObj.document.body.childNodes;
	for (let index = 0; index < frameArr.length; index++) {
		let frameObj = frameArr[index];
		if(frameObj.contentWindow) {
			let childArr = querySelectorAll(code, {winObj: frameObj.contentWindow, filterFunc: filterFunc});
			if(childArr.length > 0) {
				childArr.forEach(element => {
					items.push(element);
				});
			}
		}
	}
	return items;
}

function createScriptLinkByHead(documentObj, scriptLink, id = null) {
    if(id != null){
        if(documentObj.querySelector('#'+id))
            return;
    }

    let scriptEl = document.createElement('script');
    scriptEl.src = scriptLink;
    if(id != null)
        scriptEl.id = id;
    documentObj.head.append(scriptEl);
}

function createScriptByHead(documentObj, scriptContent, id = null) {
    if(id != null){
        if(documentObj.querySelector('#'+id))
            return;
    }

    let scriptEl = document.createElement('script');
    scriptEl.textContent = scriptContent;
    if(id != null)
        scriptEl.id = id;
    documentObj.head.append(scriptEl);
}

function createStyleLinkByHead(documentObj, styleLink, id = null) {
    if(id != null){
        if(documentObj.querySelector('#'+id))
            return;
    }

    let styleEl = document.createElement('style');
    styleEl.src = styleLink;
    if(id != null)
        scriptEl.id = id;
    documentObj.head.append(styleEl);
}

function createStyleByHead(documentObj, styleContent, id = null) {
    if(id != null){
        if(documentObj.querySelector('#'+id))
            return;
    }

    let styleEl = document.createElement('style');
    styleEl.textContent = styleContent;
    if(id != null)
        scriptEl.id = id;
    documentObj.head.append(styleEl);
}
";
        }

        private string customAlert
        {
            get => @"
function createAlertScript(documentObj, windowObj) {
    createStyleLinkByHead(documentObj,'https://cdn.jsdelivr.net/npm/sweetalert2@11.10.2/dist/sweetalert2.min.css');
    createScriptLinkByHead(documentObj,'https://cdn.jsdelivr.net/npm/sweetalert2@11.10.2/dist/sweetalert2.all.min.js', 'sweetalert');
    
    windowObj.originAlert = windowObj.alert;
    windowObj.originConfirm = windowObj.confirm;
    windowObj.alert = customAlert;
    windowObj.confirm = function(msg){console.log('msg:', msg); customAlert(msg); return true; };

    return windowObj;
}

function customAlert(message) {
    message = typeof(message) == 'object'? message: message.toString();
    if(document.querySelector('body') == null && document.querySelector('frame') != null && document.querySelector('frame').contentWindow.Swal != null) {
        document.querySelector('frame').contentWindow.Swal.fire(message);
    } else if(Swal != null) {
        Swal.fire(message);
    } else {
        alert(message);
    }
}

function initAlertScript(winObj){
    let frameObj = winObj.document.querySelector('frame');
    if(frameObj != null){
        initAlertScript(frameObj.contentWindow);
        createAlertScript(frameObj.contentDocument, frameObj.contentWindow);
        winObj.alert = frameObj.contentWindow.alert;
    }
}

function injectScript(){
    setTimeout(()=>{
        if(document.head == null)
            return injectScript();

        initAlertScript(createAlertScript(document, window));
    }, 200);
}
injectScript();
//document.addEventListener('load', function() {
//    initAlertScript(createAlertScript(document, window));
//});
";
        }

        private string makeStateFunc
        {
            get => @"
function getKysionWebHost() {
    return window.chrome.webview.hostObjects." + InjectName + @"
}
function ExecSuccessMessage_" + InjectName + @"(str) {
    window.chrome.webview.hostObjects." + InjectName + @".Success(str);
}
function ExecFailureMessage_" + InjectName + @"(str) {
    window.chrome.webview.hostObjects." + InjectName + @".Failure(str);
}
";
        }
    }
}
