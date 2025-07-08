using Kysion.Extensions.Core.Services;
using Kysion.Extensions.Core.Singleton;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Kysion.Extensions.Core.BaseAPI
{
    public class ClientAPI : IDisposable
    {
        public ILogger Logger;
        public Task<HttpResponseMessage> Response { get; set; }
        public HttpRequestMessage Request { get; set; }
        protected HttpClient HttpClient { get; set; }
        public string InterfaceName { get; set; }

        public ClientAPI(HttpRequestMessage request, string interfaceName, string? CustomAuthorizationToken = null, Uri? CustomBaseDomain = null)
        {
            InterfaceName = interfaceName;

            Logger = LoggerService.CreateLogger<ClientAPI>("网络通信");

            Logger.LogInformation("创建{interfaceName}请求", interfaceName);

            HttpClient = new HttpClient(new HttpService.ClientHandler()
            {
                AllowAutoRedirect = true,
                UseCookies = false,
                UseDefaultCredentials = true,
            });

            this.Request = request;

            var apiPath = string.Empty;

            if (!request.RequestUri!.ToString().Contains("://"))
            {
                apiPath = request.RequestUri.ToString();
                if (apiPath.StartsWith("/") && KysionConfig.Instance.BaseDomain.EndsWith("/"))
                    apiPath = apiPath[1..];
            }

            request.RequestUri = new Uri(KysionConfig.Instance.BaseDomain + apiPath);

            if (CustomBaseDomain != null)
                request.RequestUri = new Uri(CustomBaseDomain.ToString() + apiPath); ;

            if (CustomAuthorizationToken != null)
                HttpClient.DefaultRequestHeaders.Add("Authorization", CustomAuthorizationToken);
            else if (KysionConfig.Instance.TokenInfo?.Token != null)
                HttpClient.DefaultRequestHeaders.Add("Authorization", KysionConfig.Instance.TokenInfo.Token);

            HttpClient.DefaultRequestHeaders.Add("Accept", KysionConfig.Instance.Accept);
            HttpClient.DefaultRequestHeaders.Add("User-Agent", KysionConfig.Instance.UserAgent);
            HttpClient.Timeout = new TimeSpan(0, 0, 30);

            Response = HttpClient.SendAsync(request);
        }

        public void Dispose()
        {
            Logger.LogInformation("释放{InterfaceName}请求", InterfaceName);
            GC.SuppressFinalize(this);
        }
    }
}
