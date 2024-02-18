using System.Text;

namespace Kysion.Extensions.Core.Helper
{
    public static class EncryptHelper
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="isUpper">是否大写，默认大写</param>
        /// <param name="is32">是否是16位，默认32位</param>
        /// <returns></returns>
        public static string MD5(string content, bool isUpper = true, bool is32 = true)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
            string md5Str = BitConverter.ToString(result);
            md5Str = md5Str.Replace("-", "");
            md5Str = isUpper ? md5Str : md5Str.ToLower();
            return is32 ? md5Str : md5Str.Substring(8, 16);
        }

        /// <summary>
        /// Base64加密字符串
        /// </summary>
        /// <param name="source">明文</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密字符串
        /// </summary>
        /// <param name="source">明文</param>
        /// <param name="encodeType">编码类型</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string Base64Encode(string source, Encoding encodeType)
        {
            try
            {
                return Convert.ToBase64String(encodeType.GetBytes(source));
            }
            catch (Exception)
            {
                return source;
            }
        }

        /// <summary>
        /// 解密Base64字符串
        /// </summary>
        /// <param name="source">Base64字符串</param>
        /// <returns>解密后的明文</returns>
        public static string Base64Decode(string source)
        {
            return Base64Decode(source, Encoding.UTF8);
        }

        /// <summary>
        /// 解密Base64字符串
        /// </summary>
        /// <param name="source">Base64字符串</param>
        /// <param name="encodeType">编码类型</param>
        /// <returns>解密后的明文</returns>
        public static string Base64Decode(string source, Encoding encodeType)
        {
            try
            {
                return encodeType.GetString(Convert.FromBase64String(source));
            }
            catch (Exception)
            {
                return source;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <param name="Vector"></param>
        /// <returns></returns>
        public static string AESEncrypt(string Data, string Key, string Vector)
        {
            try
            {
                return AesHelper.AESEncrypt(Data, Key, Vector);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <param name="Vector"></param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] Data, string Key, string Vector)
        {
            try
            {
                return AesHelper.AESEncrypt(Data, Key, Vector);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <param name="Vector"></param>
        /// <returns></returns>
        public static string AESDecrypt(string Data, string Key, string Vector)
        {
            try
            {
                return AesHelper.AESDecrypt(Data, Key, Vector);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <param name="Vector"></param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] Data, string Key, string Vector)
        {
            try
            {
                return AesHelper.AESDecrypt(Data, Key, Vector);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES加密,无向量
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string AESEncrypt(string Data, string Key)
        {
            try
            {
                return AesHelper.AESEncrypt(Data, Key);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES加密,无向量
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] Data, string Key)
        {
            try
            {
                return AesHelper.AESEncrypt(Data, Key);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES解密,无向量
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string AESDecrypt(string Data, string Key)
        {
            try
            {
                return AesHelper.AESDecrypt(Data, Key);
            }
            catch (Exception)
            {
                return Data;
            }
        }

        /// <summary>
        /// AES解密,无向量
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] Data, string Key)
        {
            try
            {
                return AesHelper.AESDecrypt(Data, Key);
            }
            catch (Exception)
            {
                return Data;
            }
        }
    }
}
