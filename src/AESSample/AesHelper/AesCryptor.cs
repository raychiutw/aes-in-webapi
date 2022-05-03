using Microsoft.Extensions.Options;
using NETCore.Encrypt;
using System.Text.Json;

namespace AESSample.AesHelper
{
    public class AesCryptor : IAesCryptor
    {
        private readonly string _key;
        private readonly string _iv;
        private readonly JsonSerializerOptions _jsonOptions;

        public AesCryptor(IOptionsMonitor<AesOptions> options)
        {
            _key = options.CurrentValue.Key;
            _iv = options.CurrentValue.Iv;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string? Encrypt<T>(T value)
        {
            if (value == null)
            {
                return default;
            }

            var json = JsonSerializer.Serialize(value, _jsonOptions);
            return Encrypt(json);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string? Encrypt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            var encrypted = EncryptProvider.AESEncrypt(value, _key, _iv);
            return encrypted;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public T? Decrypt<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            var decrypted = Decrypt(value);
            var obj = JsonSerializer.Deserialize<T>(decrypted, _jsonOptions);

            return obj;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string? Decrypt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            var decrypted = EncryptProvider.AESDecrypt(value, _key, _iv);

            return decrypted;
        }
    }
}