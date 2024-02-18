using Kysion.Extensions.Core.Services;
using Kysion.Extensions.Core.Singleton;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Kysion.Extensions.Core.BaseAPI
{
    public class ClientAPI : IDisposable
    {
        public ILogger Logger;
        public Task<HttpResponseMessage> response { get; set; }
        public HttpRequestMessage request { get; set; }
        protected HttpClient httpClient { get; set; }
        public string InterfaceName { get; set; }

        public ClientAPI(HttpRequestMessage request, string interfaceName, string? CustomAuthorizationToken = null, Uri? CustomBaseDomain = null)
        {
            InterfaceName = interfaceName;

            Logger = LoggerService.CreateLogger<ClientAPI>("网络通信");

            Logger.LogInformation("创建" + interfaceName + "请求");

            httpClient = new HttpClient(new HttpService.ClientHandler()
            {
                AllowAutoRedirect = true,
                UseCookies = false,
                UseDefaultCredentials = true,
            });

            this.request = request;

            var apiPath = string.Empty;

            if (!request.RequestUri!.ToString().Contains("://"))
            {
                apiPath = request.RequestUri.ToString();
                if (apiPath.StartsWith("/") && KysionConfig.Instance.BaseDomain.EndsWith("/"))
                    apiPath = apiPath.Substring(1);
            }

            request.RequestUri = new Uri(KysionConfig.Instance.BaseDomain + apiPath);

            if (CustomBaseDomain != null)
                request.RequestUri = new Uri(CustomBaseDomain.ToString() + apiPath); ;

            if (CustomAuthorizationToken != null)
                httpClient.DefaultRequestHeaders.Add("Authorization", CustomAuthorizationToken);
            else if (KysionConfig.Instance.TokenInfo?.Token != null)
                httpClient.DefaultRequestHeaders.Add("Authorization", KysionConfig.Instance.TokenInfo.Token);

            httpClient.DefaultRequestHeaders.Add("Accept", KysionConfig.Instance.Accept);
            httpClient.DefaultRequestHeaders.Add("User-Agent", KysionConfig.Instance.UserAgent);
            httpClient.Timeout = new TimeSpan(0, 0, 30);

            response = httpClient.SendAsync(request);
        }

        public void Dispose()
        {
            httpClient.Dispose();
            Logger.LogInformation("释放" + InterfaceName + "请求");
        }
    }
}
