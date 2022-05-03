using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AESSample.AesHelper
{
    public class AesModelBinder : IModelBinder
    {
        private readonly IAesCryptor _cryptor;

        public AesModelBinder(IAesCryptor cryptor)
        {
            _cryptor = cryptor;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            using var reader = new StreamReader(bindingContext.HttpContext.Request.Body);
            var body = await reader.ReadToEndAsync();

            var parameter = _cryptor.GetType()
                .GetMethods()
                .First(x => x.IsGenericMethod.Equals(true) &&
                            x.Name.Equals(nameof(_cryptor.Decrypt)))
                .MakeGenericMethod(bindingContext.ModelType)
                .Invoke(_cryptor, new object[] { body });

            bindingContext.Result = ModelBindingResult.Success(parameter);
        }
    }
}