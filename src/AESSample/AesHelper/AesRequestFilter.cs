using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace AESSample.AesHelper
{
    /// <summary>
    /// Logging ActionFilter
    /// </summary>
    /// <seealso cref="IAsyncActionFilter"/>
    public class AesRequestFilter : IAsyncResourceFilter
    {
        private readonly IAesCryptor _cryptor;

        public AesRequestFilter(IAesCryptor cryptor)
        {
            _cryptor = cryptor;
        }

        public async Task OnResourceExecutionAsync(
            ResourceExecutingContext context,
            ResourceExecutionDelegate next)
        {
            using var reader = new StreamReader(context.HttpContext.Request.Body);
            var body = await reader.ReadToEndAsync();

            var json = _cryptor.Decrypt(body);

            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            context.HttpContext.Request.ContentType = "application/json";
            context.HttpContext.Request.Body = await requestContent.ReadAsStreamAsync(); ;

            await next();
        }
    }
}