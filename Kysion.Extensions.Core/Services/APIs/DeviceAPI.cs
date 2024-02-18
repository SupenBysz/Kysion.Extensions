using Kysion.Extensions.Core.BaseAPI;
using Kysion.Extensions.Core.Singleton;
using Kysion.Extensions.Core.Helper;
using Kysion.Extensions.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Kysion.Extensions.Core.Services.APIs
{
    public abstract class InnerDeviceAPI
    {
        public static async Task<LicenseInfo?> GetDeviceRegister()
        {
            var requset = new HttpRequestMessage(HttpMethod.Post, "/api/getLicense/get-device-register");
            var data = new Dictionary<string, object>(){
                { "identifier", KysionConfig.Instance.HardwareUUID },
                { "licenseType", KysionConfig.Instance.DefaultLicenseType}
            };
            requset.Content = new StringContent(JsonConvert.SerializeObject(data));

            try
            {
                using (var cli = new ClientAPI(requset, "System"))
                {
                    try
                    {
                        var response = await cli.response.WaitAsync(new CancellationToken());
                        var body = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<ResponseBody<string>>(body);

                        if (responseData != null)
                        {
                            var aesBytes = Convert.FromHexString(responseData.Data!);
                            var key = EncryptHelper.MD5(KysionConfig.Instance.HardwareUUID, false);
                            var iv = EncryptHelper.MD5(KysionConfig.Instance.HardwareUUID, false).Substring(8, 16);
                            var plaintextBytes = EncryptHelper.AESDecrypt(aesBytes, key, iv);
                            var plaintext = Encoding.UTF8.GetString(plaintextBytes);
                            var jsonBytes = Convert.FromBase64String(plaintext);
                            var jsonString = Encoding.UTF8.GetString(jsonBytes);
                            var licenseObj = JsonConvert.DeserializeObject<LicenseInfo>(jsonString);

                            if (licenseObj != null)
                            {
                                KysionConfig.Instance.LicenseInfo = licenseObj;
                                cli.Logger.LogInformation("机器码 [" + KysionConfig.Instance.HardwareUUID + "] 加载成功");
                            }
                            return licenseObj;
                        }

                        Debug.WriteLine(responseData);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
