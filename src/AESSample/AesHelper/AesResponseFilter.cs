using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AESSample.AesHelper
{
    /// <summary>
    /// Logging ActionFilter
    /// </summary>
    /// <seealso cref="IAsyncActionFilter"/>
    public class AesResponseFilter : IAsyncActionFilter
    {
        private readonly IAesCryptor _cryptor;

        public AesResponseFilter(IAesCryptor cryptor)
        {
            _cryptor = cryptor;
        }

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext"/>.</param>
        /// <param name="next">
        /// The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate"/>. Invoked to
        /// execute the next action filter or the action itself.
        /// </param>
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var executedContext = await next();

            var aesResponse = string.Empty;

            if (executedContext.Result is ObjectResult result)
            {
                aesResponse = _cryptor.Encrypt(result.Value);
            }

            context.HttpContext.Response.ContentType = "text/plain";
            context.HttpContext.Response.StatusCode = 200;

            await context.HttpContext.Response.WriteAsync(aesResponse ?? "");
        }
    }
}