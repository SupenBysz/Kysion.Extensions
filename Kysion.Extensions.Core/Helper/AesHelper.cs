using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kysion.Extensions.Core.Helper
{
    public class AesHelper
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string Data, string Key, string Vector)
        {
            var plainBytes = Encoding.UTF8.GetBytes(Data);
            var cryptograph = AESEncrypt(plainBytes, Key, Vector);
            return Convert.ToBase64String(cryptograph);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <param name="Key"></param>
        /// <param name="Vector"></param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] plainBytes, string Key, string Vector)
        {
            var bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            var bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            var cryptograph = Array.Empty<byte>(); // 加密后的密文

            var aes = Aes.Create();
            try
            {
                // 开辟一块内存流
                using var Memory = new MemoryStream();
                // 把内存流对象包装成加密流对象
                using (var Encryptor = new CryptoStream(Memory, aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                {
                    // 明文数据写入加密流
                    Encryptor.Write(plainBytes, 0, plainBytes.Length);
                    Encryptor.FlushFinalBlock();

                    cryptograph = Memory.ToArray();
                    Encryptor.Close();
                }
                Memory.Close();
            }
            catch
            {
                //
            }
            return cryptograph;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string Data, string Key, string Vector)
        {
            var encryptedBytes = Convert.FromBase64String(Data);
            var original = AESDecrypt(encryptedBytes, Key, Vector);
            return Encoding.UTF8.GetString(original);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>明文</returns>
        public static byte[] AESDecrypt(byte[] encryptedBytes, string Key, string Vector)
        {
            var bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            var bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            var original = Array.Empty<byte>(); // 解密后的明文

            var aes = Aes.Create();
            try
            {
                // 开辟一块内存流，存储密文
                using var Memory = new MemoryStream(encryptedBytes);
                // 把内存流对象包装成加密流对象
                using (var Decryptor = new CryptoStream(Memory, aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                {
                    // 明文存储区
                    using (var originalMemory = new MemoryStream())
                    {
                        var Buffer = new byte[1024];
                        var readBytes = 0;
                        while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                        {
                            originalMemory.Write(Buffer, 0, readBytes);
                        }

                        original = originalMemory.ToArray();
                        originalMemory.Close();
                    }
                    Decryptor.Clear();
                    Decryptor.Close();
                }
                Memory.Close();
            }
            catch
            {
                aes.Clear();
                aes.Dispose();
            }
            return original;
        }



        /// <summary>
        /// AES加密(无向量)
        /// </summary>
        /// <param name="plainBytes">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string Data, string Key)
        {
            var plainBytes = Encoding.UTF8.GetBytes(Data);
            var cryptograph = AESEncrypt(plainBytes, Key);
            return Encoding.UTF8.GetString(cryptograph);
        }

        /// <summary>
        /// AES加密(无向量)
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] plainBytes, string Key)
        {
            var mStream = new MemoryStream();
            var aes = Aes.Create();

            var cryptograph = Array.Empty<byte>(); // 加密后的密文

            var bKey = new byte[32];
            Key = Key.PadRight(bKey.Length);
            Array.Copy(Encoding.UTF8.GetBytes(Key), bKey, bKey.Length);

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            //aes.Key = _key;
            aes.Key = bKey;
            //aes.IV = _iV;
            var cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                cryptograph = mStream.ToArray();
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
            return cryptograph;
        }


        /// <summary>
        /// AES解密(无向量)
        /// </summary>
        /// <param name="encryptedBytes">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string Data, string Key)
        {
            var encryptedBytes = Convert.FromBase64String(Data);
            var original = AESDecrypt(encryptedBytes, Key);
            return Encoding.UTF8.GetString(original);
        }

        /// <summary>
        /// AES解密(无向量)
        /// </summary>
        /// <param name="encryptedBytes"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] encryptedBytes, string Key)
        {
            var bKey = new byte[32];
            Key = Key.PadRight(bKey.Length);
            Array.Copy(Encoding.UTF8.GetBytes(Key), bKey, bKey.Length); ;

            var original = Array.Empty<byte>(); // 解密后的明文
            var mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );
            //mStream.Seek( 0, SeekOrigin.Begin );
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            //aes.IV = _iV;
            var cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                var tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                var ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                original = ret;
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
            return original;
        }
    }
}
