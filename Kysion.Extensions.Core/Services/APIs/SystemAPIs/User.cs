using Kysion.Extensions.Core.BaseAPI;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace Kysion.Extensions.Core.Services.APIs.SystemAPIs
{
    public class User
    {
        public static async void Login()
        {
            var requset = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login");
            requset.Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    { "username", "" },
                    { "password", "" },
                    { "captcha", "" },
                })); ;

            try
            {
                using (var api = new ClientAPI(requset, "User"))
                {
                    var response = await api.Response.WaitAsync(new CancellationToken());
                    Debug.WriteLine(response.Content);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
