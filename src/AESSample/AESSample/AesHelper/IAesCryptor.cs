namespace AESSample.AesHelper
{
    /// <summary>
    /// 加密器
    /// </summary>
    public interface IAesCryptor
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string? Encrypt<T>(T value);

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string? Encrypt(string value);

        /// <summary>
        /// 解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public T? Decrypt<T>(string value);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string? Decrypt(string value);
    }
}