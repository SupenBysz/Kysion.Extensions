using Kysion.Extensions.Core.Services.APIs;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Kysion.Extensions.Core.Services
{
    public class HttpService
    {
        HttpService() { }

        internal class ClientHandler : HttpClientHandler
        {
            internal static string token { get; set; } = string.Empty;
            private void SetAuthorization(HttpRequestMessage request)
            {
                if (!request.Headers.Contains("Authorization") && token != string.Empty)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Beaer", token);
                }
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                try
                {
                    SetAuthorization(request);
                    return base.SendAsync(request, cancellationToken);
                }
                catch (Exception)
                {
                    return Task.Run(() =>
                    {
                        return new HttpResponseMessage();
                    });
                }
            }

            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                SetAuthorization(request);
                return base.Send(request, cancellationToken);
            }
        }

        public class DeviceAPI : InnerDeviceAPI
        {

        }
    }
}
